using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Services.Ecommerce.Lib.Services;
using Mix.Shared.Interfaces;

namespace Mix.Services.Ecommerce.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMixOnepay();
            services.AddMixEcommerce();
            services.TryAddScoped<OnepayService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
