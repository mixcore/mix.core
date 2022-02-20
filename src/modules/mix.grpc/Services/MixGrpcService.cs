using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Mix.Grpc
{
    public class MixGrpcService : MixGrpc.MixGrpcBase
    {
        private readonly ILogger<MixGrpcService> _logger;
        public MixGrpcService(ILogger<MixGrpcService> logger)
        {
            _logger = logger;
        }

        public override Task<MixGrpcReply> Send(MixGrpcRequest request, ServerCallContext context)
        {
            // From Module in MixModuleNames
            return Task.FromResult(new MixGrpcReply
            {
                Message = @$"Hello {request.To} from {request.From} <br>
                            Please Override MixGrpcService to handle message"
            });
        }

        [Authorize]
        public override Task<MixGrpcReply> SendAuthorized(MixGrpcRequest request, ServerCallContext context)
        {
            return Task.FromResult(new MixGrpcReply
            {
                Message = @$"Hello {request.Type} from {Assembly.GetEntryAssembly().FullName} 
                                    by {context.GetHttpContext().User.Identity.Name} <br>
                            Please Override MixGrpcService to handle message"
            });
        }

        public override Task<MixGrpcReply> SendStream(IAsyncStreamReader<MixGrpcRequest> requestStream, ServerCallContext context)
        {
            return base.SendStream(requestStream, context);
        }

        public override async Task GetStream(
            MixGrpcRequest request,
            IServerStreamWriter<MixGrpcReply> responseStream,
            ServerCallContext context)
        {
            var i = 0;
            while (!context.CancellationToken.IsCancellationRequested && i < 20)
            {
                await Task.Delay(1000);

                var forecast = new MixGrpcReply
                {
                    Message = DateTime.Now.ToString("hh-mm-ss")
                };

                _logger.LogInformation("Sending response");

                await responseStream.WriteAsync(forecast);
            }
        }

    }
}
