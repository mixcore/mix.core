using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
namespace Mix.Cms.Service.Gprc
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = $"Hello {request.Name}" });
        }

        public override async Task SayHellos(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            var i = 0;
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new HelloReply { Message = $"Hello {request.Name} {i}" });
                await Task.Delay(TimeSpan.FromSeconds(1));
                i++;
            }
        }
    }
}
