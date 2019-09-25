# Audio streaming with gRPC and C# 8 async streams 

The server reads a wave file from disk and creates a stream of audio samples from it. The audio samples
are then streamed to the client using gRPC. On the client side C# 8 async streams are used to asynchronously
consume the stream of samples and feed them into an audio player.

## Resources

- [gRPC for ASP.NET Core 3.0](https://mustak.im/grpc-for-asp-net-core-3-0/)
- [gRPC and C# 8 Async stream](https://laurentkempe.com/2019/09/18/gRPC-and-csharp-8-Async-stream/?utm_source=feedburner&utm_medium=feed&utm_campaign=Feed%3A+laurentkempe+%28Laurent+Kemp%C3%A9%29)
- [Introduction to gRPC on .NET Core](https://docs.microsoft.com/en-us/aspnet/core/grpc/index?view=aspnetcore-3.0)
- [gRPC for .NET Github Repository](https://github.com/grpc/grpc-dotnet)
- [gRPC Website](https://grpc.io/)
- [Protocol Buffers Website](https://developers.google.com/protocol-buffers)
- [NAudio](https://github.com/naudio/NAudio)