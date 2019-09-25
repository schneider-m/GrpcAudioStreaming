using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace GrpcAudioStreaming.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // When calling insecure gRPC services this switch must be set before creating the GrpcChannel/HttpClient.
            // https://docs.microsoft.com/en-us/aspnet/core/grpc/troubleshoot?view=aspnetcore-3.0#call-insecure-grpc-services-with-net-core-client
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // The port number(5001) must match the port of the gRPC server.
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new AudioStream.AudioStreamClient(channel);
            var format = client.GetFormat(new Empty());
            var audioStream = client.GetStream(new Empty());

            using var audioPlayer = new AudioPlayer(format.ToWaveFormat());
            audioPlayer.Play();

            await foreach (var sample in audioStream.ResponseStream.ReadAllAsync())
            {
                audioPlayer.AddSample(sample.Data.ToByteArray());
            }
        }
    }
}