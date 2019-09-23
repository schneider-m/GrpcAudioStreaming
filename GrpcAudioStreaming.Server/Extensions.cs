using NAudio.Wave;

namespace GrpcAudioStreaming.Server
{
    public static class Extensions
    {
        public static AudioFormat ToAudioFormat(this WaveFormat waveFormat)
        {
            return new AudioFormat
            {
                AverageBytesPerSecond = waveFormat.AverageBytesPerSecond,
                BitsPerSample = waveFormat.BitsPerSample,
                BlockAlign = waveFormat.BlockAlign,
                Channels = waveFormat.Channels,
                ExtraSize = waveFormat.ExtraSize,
                SampleRate = waveFormat.SampleRate,
                Encoding = waveFormat.Encoding.ToString()
            };
        }
    }
}