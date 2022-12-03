using Mix.Shared.Interfaces;

namespace Mix.Services.Ecommerce.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMixPayment();
            services.AddMixOnepay();
            services.AddMixEcommerce();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
