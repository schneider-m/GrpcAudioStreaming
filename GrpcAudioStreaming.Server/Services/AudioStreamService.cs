using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GrpcAudioStreaming.Server.Services
{
    public class AudioStreamService : AudioStream.AudioStreamBase
    {
        private readonly IAudioSampleSource _audioSampleSource;
        private IServerStreamWriter<AudioSample> _responseStream;

        public AudioStreamService(IAudioSampleSource audioSampleSource)
        {
            _audioSampleSource = audioSampleSource;
        }

        public override Task GetStream(Empty request, IServerStreamWriter<AudioSample> responseStream,
            ServerCallContext context)
        {
            _responseStream = responseStream;
            _audioSampleSource.AudioSampleCreated += async (_, audioSample) => await _responseStream.WriteAsync(audioSample);
            return _audioSampleSource.StartStreaming();
        }

        public override Task<AudioFormat> GetFormat(Empty request, ServerCallContext context)
        {
            return Task.FromResult(_audioSampleSource.AudioFormat);
        }
    }
}