using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Constant.Constants;
using Mix.Lib.Extensions;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Services.Databases.Lib.Services;
using Mix.Shared.Interfaces;
using Mix.Shared.Models.Configurations;

namespace Mix.Services.Ecommerce
{
    public class StartupService : IStartupService
    {
        public void AddServices(IHostApplicationBuilder builder)
        {
            var globalSettings = builder.Configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>();

            if (!builder.Configuration.IsInit())
            {
                builder.AddMixEcommerce();
                builder.Services.TryAddScoped<IMixMetadataService, MixMetadataService>();
                
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
