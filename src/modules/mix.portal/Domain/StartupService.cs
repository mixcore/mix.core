using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Portal.Domain.Subscribers;
using Mix.Portal.Publishers;

namespace Mix.Portal
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<ThemePublisherService>();
            services.AddHostedService<ThemeSubscriberService>();
            services.AddScoped<MixThemeExportService>();
            services.AddScoped<MixThemeImportService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
