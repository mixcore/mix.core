using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Shared.Interfaces;
using Mix.Shared.Models.Configurations;
using Mix.Tenancy.Domain.Interfaces;
using Mix.Tenancy.Domain.Services;

namespace Mix.Tenancy.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<EntityRepository<MixCmsContext, MixConfiguration, int>>();
            builder.Services.AddScoped<EntityRepository<MixCmsContext, MixLanguage, int>>();
            builder.Services.AddScoped<EntityRepository<MixCmsContext, MixPost, int>>();
            builder.Services.AddScoped<EntityRepository<MixCmsContext, MixPage, int>>();
            builder.Services.AddScoped<EntityRepository<MixCmsContext, MixModule, int>>();

            builder.Services.AddScoped<IInitCmsService, InitCmsService>();
            builder.Services.AddScoped<IImportSiteService, ImportSiteService>();
            builder.Services.AddScoped<IMixThemeExportService, MixThemeExportService>();
            builder.Services.AddScoped<IMixThemeImportService, MixThemeImportService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
