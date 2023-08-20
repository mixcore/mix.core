using Mix.Lib.Interfaces;
using Mix.Storage.Lib.Extensions;

namespace Mix.Storage
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMixStorage();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
