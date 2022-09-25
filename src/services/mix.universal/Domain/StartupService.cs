using Mix.Shared.Interfaces;
using Mix.Universal.Lib.Entities;

namespace Mix.Universal.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MixUniversalDbContext>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
