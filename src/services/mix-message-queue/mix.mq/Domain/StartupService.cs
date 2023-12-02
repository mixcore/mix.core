using Mix.Lib.Interfaces;
using Mix.Mq.Domain.Services;

namespace Mix.Mq.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpc();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
            endpoints.MapGrpcService<MixMqService>();
        }
    }
}
