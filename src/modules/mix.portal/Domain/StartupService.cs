using Mix.Lib.Interfaces;
using Mix.Portal.Publishers;

namespace Mix.Portal
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            // Queue publisher
            services.AddHostedService<ThemePublisherService>();
            services.AddHostedService<TemplatePublisherService>();
            services.AddHostedService<PageContentPublisherService>();

            services.AddScoped<MixThemeExportService>();
            services.AddScoped<MixThemeImportService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
