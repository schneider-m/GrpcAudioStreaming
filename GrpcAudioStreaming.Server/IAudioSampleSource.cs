using System;
using System.Threading.Tasks;

namespace GrpcAudioStreaming.Server
{
    public interface IAudioSampleSource
    {
        event EventHandler<AudioSample> AudioSampleCreated;

        AudioFormat AudioFormat { get; }

        Task StartStreaming();
        void StopStreaming();
    }
}