using System;
using System.Diagnostics;
using NAudio.Wave;

namespace GrpcAudioStreaming.Client
{
    public class AudioPlayer : IDisposable
    {
        private readonly IWavePlayer _wavePlayer;
        private readonly BufferedWaveProvider _bufferedWaveProvider;

        public AudioPlayer(WaveFormat waveFormat)
        {
            _wavePlayer = new WaveOutEvent();
            _bufferedWaveProvider = new BufferedWaveProvider(waveFormat) {BufferDuration = TimeSpan.FromSeconds(5)};
            _wavePlayer.Init(_bufferedWaveProvider);
        }

        public void AddSample(byte[] sample)
        {
            try
            {
                _bufferedWaveProvider.AddSamples(sample, 0, sample.Length);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Adding samples failed: {ex}");
            }
        }

        public void Play()
        {
            _wavePlayer.Play();
        }

        public void Stop()
        {
            _wavePlayer.Stop();
        }

        public void Dispose()
        {
            _wavePlayer.Stop();
            _wavePlayer.Dispose();
        }
    }
}