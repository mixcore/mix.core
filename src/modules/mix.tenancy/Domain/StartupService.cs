using Mix.Lib.Services;
using Mix.Shared.Interfaces;
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


            services.AddScoped<InitCmsService>();
            services.AddScoped<ImportSiteService>();
            services.AddScoped<MixThemeExportService>();
            services.AddScoped<MixThemeImportService>();


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
    }
}
