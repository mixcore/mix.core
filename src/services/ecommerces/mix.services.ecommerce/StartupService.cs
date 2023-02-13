using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Services.Databases.Lib.Services;
using Mix.Services.Ecommerce.Lib.Services;
using Mix.Shared.Interfaces;

namespace Mix.Services.Ecommerce
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMixOnepay();
            services.AddMixEcommerce();
            services.TryAddScoped<MixMetadataService>();
            services.TryAddScoped<ProductService>();
            services.TryAddScoped<OnepayService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
