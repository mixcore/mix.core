using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Portal.Domain.Services;
using Mix.Shared.Interfaces;

namespace Mix.Portal
{
    public sealed class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<MixThemeExportService>();
            services.AddScoped<MixThemeImportService>();
            services.TryAddScoped<CloneCultureService>();
            services.TryAddScoped<ThemeService>();
            services.TryAddScoped<MixApplicationService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
