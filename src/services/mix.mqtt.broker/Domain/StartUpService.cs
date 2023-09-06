using Mix.Lib.Interfaces;
using MQTTnet.AspNetCore;

namespace mix.mqtt.broker.Domain
{
    public class StartUpService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
