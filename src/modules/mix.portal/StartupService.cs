using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Portal.Domain.Services;
using Mix.Shared.Interfaces;

namespace Mix.Portal
{
    public sealed class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<MixApplicationService>();
            services.AddScoped<IMixThemeExportService, MixThemeExportService>();
            services.AddScoped<IMixThemeImportService, MixThemeImportService>();
            services.TryAddScoped<CloneCultureService>();
            services.TryAddScoped<ThemeService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
