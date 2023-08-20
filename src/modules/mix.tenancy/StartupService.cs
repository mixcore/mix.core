using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Tenancy.Domain.Interfaces;
using Mix.Tenancy.Domain.Services;

namespace Mix.Tenancy
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


            if (GlobalConfigService.Instance.InitStatus == InitStep.Blank)
            {
                if (string.IsNullOrEmpty(GlobalConfigService.Instance.AppSettings.ApiEncryptKey))
                {
                    GlobalConfigService.Instance.AppSettings.ApiEncryptKey = AesEncryptionHelper.GenerateCombinedKeys();
                    GlobalConfigService.Instance.SaveSettings();
                }
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
