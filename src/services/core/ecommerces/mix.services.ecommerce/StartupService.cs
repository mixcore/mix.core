using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Constant.Constants;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Services.Databases.Lib.Services;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.Models;
using Mix.Services.Ecommerce.Lib.Services;
using Mix.Shared.Interfaces;
using Mix.Shared.Models.Configurations;

namespace Mix.Services.Ecommerce
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            var globalSettings = configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>();

            if (!globalSettings!.IsInit)
            {
                services.AddMixEcommerce(configuration);
                services.TryAddScoped<IMixMetadataService, MixMetadataService>();
                
            }
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
