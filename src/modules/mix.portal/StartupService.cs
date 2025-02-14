using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Lib.Interfaces;
using Mix.Portal.Domain.Interfaces;
using Mix.Portal.Domain.Services;
using Mix.Shared.Interfaces;

namespace Mix.Portal
{
    public sealed class StartupService : IStartupService
    {
        public void AddServices(IHostApplicationBuilder builder)
        {
            builder.Services.TryAddScoped<IMixApplicationService, MixApplicationService>();
            builder.Services.TryAddScoped<PortalPostService>();
            builder.Services.AddScoped<IMixThemeExportService, MixThemeExportService>();
            builder.Services.AddScoped<IMixThemeImportService, MixThemeImportService>();
            builder.Services.TryAddScoped<ICloneCultureService, CloneCultureService>();
            builder.Services.TryAddScoped<IThemeService, ThemeService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
