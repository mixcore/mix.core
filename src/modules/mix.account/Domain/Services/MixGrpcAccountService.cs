using Grpc.Core;
using Microsoft.Extensions.Logging;
using Mix.Grpc;
using System.Threading.Tasks;

namespace Mix.Account.Domain.Services
{
    public class MixGrpcAccountService : MixGrpcrService
    {
        public MixGrpcAccountService(ILogger<MixGrpcrService> logger) : base(logger)
        {
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override Task<MixGrpcReply> Send(MixGrpcRequest request, ServerCallContext context)
        {
            return base.Send(request, context);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
