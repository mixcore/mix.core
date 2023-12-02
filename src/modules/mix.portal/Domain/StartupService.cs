using Mix.Lib.Interfaces;
using Mix.Portal.Domain.Interfaces;
using Mix.Portal.Domain.Services;

namespace Mix.Portal.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICloneCultureService, CloneCultureService>();
            services.AddScoped<PortalPostService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
