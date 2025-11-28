using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Grpc.Domain.Services;
using Mix.Lib.Interfaces;
using Mix.Shared.Interfaces;

namespace Mix.Grpc.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IHostApplicationBuilder builder)
        {
            builder.Services.AddGrpc();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
            endpoints.MapGrpcService<MixGrpcService>();
        }
    }
}
