using Mix.Lib.Interfaces;
using Mix.Lib.Publishers;
using Mix.Lib.ViewModels;

namespace Mix.Portal
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            // Queue publisher
            //services.AddHostedService<MixPublisher<MixThemeViewModel>>();
            //services.AddHostedService<MixPublisher<MixTemplateViewModel>>();
            //services.AddHostedService<MixPublisher<MixPageContentViewModel>>();

            services.AddScoped<MixThemeExportService>();
            services.AddScoped<MixThemeImportService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
