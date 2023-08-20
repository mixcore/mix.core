using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Lib.Interfaces;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Services.Databases.Lib.Services;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.Services;

namespace Mix.Services.Ecommerce
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMixEcommerce();
            services.TryAddScoped<IMixMetadataService, MixMetadataService>();
            services.TryAddScoped<OnepayService>();
            services.TryAddScoped<PaypalService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
