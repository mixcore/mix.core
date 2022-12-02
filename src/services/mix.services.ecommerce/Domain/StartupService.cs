using Mix.Shared.Interfaces;

namespace mix.services.ecommerce.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMixEcommerce();
            services.AddMixOnepay();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
