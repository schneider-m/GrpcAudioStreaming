using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using NAudio.Wave;

namespace GrpcAudioStreaming.Server
{
    public class AudioSampleSource : IAudioSampleSource, IDisposable
    {
        public event EventHandler<AudioSample> AudioSampleCreated;
        private readonly WaveFileReader _waveFileReader;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public AudioFormat AudioFormat { get; }

        public AudioSampleSource(string file)
        {
            _waveFileReader = new WaveFileReader(file);
            AudioFormat = _waveFileReader.WaveFormat.ToAudioFormat();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public Task StartStreaming()
        {
            return Task.Factory.StartNew(() => Stream(_waveFileReader, _cancellationTokenSource.Token),
                TaskCreationOptions.LongRunning);
        }

        public void StopStreaming()
        {
            _cancellationTokenSource.Cancel();
        }

        private void Stream(WaveStream stream, CancellationToken cancellationToken)
        {
            var buffer = new byte[stream.WaveFormat.AverageBytesPerSecond];
            var streamTimeStart = stream.CurrentTime;
            var realTimeStart = DateTime.UtcNow;

            while (!cancellationToken.IsCancellationRequested)
            {
                var bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead == 0)
                {
                    // if we have reached the end, reset stream to start
                    stream.CurrentTime = TimeSpan.Zero;
                    streamTimeStart = stream.CurrentTime;
                    realTimeStart = DateTime.UtcNow;
                    continue;
                }

                var time = realTimeStart + stream.CurrentTime;
                var audioSample = new AudioSample
                {
                    Timestamp = time.ToString("o"),
                    Data = ByteString.CopyFrom(buffer)
                };
                OnAudioSampleCreated(audioSample);

                var streamTimePassed = stream.CurrentTime - streamTimeStart;
                var realTimePassed = DateTime.UtcNow - realTimeStart;
                var timeDifference = Math.Max(0, (streamTimePassed - realTimePassed).TotalMilliseconds);
                Thread.Sleep((int)timeDifference);
            }
        }

        protected virtual void OnAudioSampleCreated(AudioSample audioSample)
        {
            AudioSampleCreated?.Invoke(this, audioSample);
        }

        public void Dispose()
        {
            _waveFileReader.Dispose();
        }
    }
}