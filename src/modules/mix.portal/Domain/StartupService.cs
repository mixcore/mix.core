using Mix.Lib.Interfaces;

namespace Mix.Portal
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<MixThemeExportService>();
            services.AddScoped<MixThemeImportService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
