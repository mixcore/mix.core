using Mix.Lib.Services;
using Mix.Shared.Interfaces;
using Mix.Tenancy.Domain.Services;

namespace Mix.Tenancy.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<InitCmsService>();
            services.AddScoped<ImportSiteService>();
            services.AddScoped<MixThemeExportService>();
            services.AddScoped<MixThemeImportService>();
            if (GlobalConfigService.Instance.InitStatus == Shared.Enums.InitStep.Blank)
            {
                GlobalConfigService.Instance.AppSettings.ApiEncryptKey = AesEncryptionHelper.GenerateCombinedKeys();
                GlobalConfigService.Instance.SaveSettings();
            }
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
