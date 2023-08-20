using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Lib.Interfaces;
using Mix.Portal.Domain.Interfaces;
using Mix.Portal.Domain.Services;

namespace Mix.Portal
{
    public sealed class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<IMixApplicationService, MixApplicationService>();
            services.TryAddScoped<PortalPostService>();
            services.AddScoped<IMixThemeExportService, MixThemeExportService>();
            services.AddScoped<IMixThemeImportService, MixThemeImportService>();
            services.TryAddScoped<ICloneCultureService, CloneCultureService>();
            services.TryAddScoped<IThemeService, ThemeService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
