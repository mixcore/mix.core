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
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<EntityRepository<MixCmsContext, MixConfiguration, int>>();
            services.AddScoped<EntityRepository<MixCmsContext, MixLanguage, int>>();
            services.AddScoped<EntityRepository<MixCmsContext, MixPost, int>>();
            services.AddScoped<EntityRepository<MixCmsContext, MixPage, int>>();
            services.AddScoped<EntityRepository<MixCmsContext, MixModule, int>>();


            services.AddScoped<IInitCmsService, InitCmsService>();
            services.AddScoped<IImportSiteService, ImportSiteService>();
            services.AddScoped<IMixThemeExportService, MixThemeExportService>();
            services.AddScoped<IMixThemeImportService, MixThemeImportService>();

        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
