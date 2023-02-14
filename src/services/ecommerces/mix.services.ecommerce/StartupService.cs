using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Services.Databases.Lib.Services;
using Mix.Services.Ecommerce.Lib.Interfaces;
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
            services.TryAddScoped<IMixMetadataService, MixMetadataService>();
            services.TryAddScoped<IProductService, ProductService>();
            services.TryAddScoped<IPaymentService, OnepayService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
