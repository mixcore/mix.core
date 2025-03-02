using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Lib.Extensions;
using Mix.Lib.Middlewares;
using Mix.Lib.Publishers;
using Mix.Lib.Subscribers;
using Mix.Log.Lib.Interfaces;
using Mix.Log.Lib.Models;
using Mix.Log.Lib.Publishers;
using Mix.Log.Lib.Services;
using Mix.Mixdb.Publishers;
using Mix.Mixdb.Subscribers;
using Mix.Quartz.Services;
using Mix.Shared.Interfaces;
using Mix.Shared.Models.Configurations;
using Mix.Storage.Lib.Subscribers;

namespace Mixcore.Domain
{
    public class StartupServices : IStartupService
    {
        public void AddServices(IHostApplicationBuilder builder)
        {
            var globalConfigs = builder.Configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>()!;
            builder.Services.AddMixRoutes();

            builder.Services.AddHostedService<MixRepoDbPublisher>();
            builder.Services.AddHostedService<MixRepoDbSubscriber>();
            builder.Services.AddHostedService<MixViewModelChangedPublisher>();
            builder.Services.AddHostedService<MixViewModelChangedSubscriber>();

            builder.Services.AddHostedService<MixBackgroundTaskPublisher>();
            builder.Services.AddHostedService<MixBackgroundTaskSubscriber>();
            builder.Services.AddHostedService<MixDbCommandPublisher>();
            builder.Services.AddHostedService<MixDbCommandSubscriber>();

            builder.Services.AddHostedService<MixQuartzHostedService>();
            builder.Services.AddHostedService<StorageBackgroundTaskSubscriber>();

            if (!builder.Configuration.IsInit())
            {
                builder.Services.TryAddSingleton<IAuditLogService, AuditLogService>();
                builder.Services.TryAddScoped<AuditLogDataModel>();
                builder.Services.AddHostedService<MixLogPublisher>();
            }

            builder.AddMixRateLimiter();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
            // auditlog middleware must go after auth
            app.UseMiddleware<AuditlogMiddleware>();
            app.UseMixRateLimiter();
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
            endpoints.UseMixMVCEndpoints();
        }
    }
}
