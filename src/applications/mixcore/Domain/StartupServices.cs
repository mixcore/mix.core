using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Lib.Middlewares;
using Mix.Lib.Publishers;
using Mix.Lib.Subscribers;
using Mix.Log.Lib.Interfaces;
using Mix.Log.Lib.Models;
using Mix.Log.Lib.Publishers;
using Mix.Log.Lib.Services;
using Mix.Log.Lib.Subscribers;
using Mix.Quartz.Interfaces;
using Mix.Quartz.Services;
using Mix.RepoDb.Publishers;
using Mix.RepoDb.Subscribers;
using Mix.Shared.Interfaces;
using Mix.Shared.Models.Configurations;
using Mix.Storage.Lib.Subscribers;

namespace Mixcore.Domain
{
    public class StartupServices : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            var globalConfigs = configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>()!;
            services.AddMixRoutes();

            services.AddHostedService<MixRepoDbPublisher>();
            services.AddHostedService<MixRepoDbSubscriber>();
            services.AddHostedService<MixViewModelChangedPublisher>();
            services.AddHostedService<MixViewModelChangedSubscriber>();

            services.AddHostedService<MixBackgroundTaskPublisher>();
            services.AddHostedService<MixBackgroundTaskSubscriber>();
            services.AddHostedService<MixDbCommandPublisher>();
            services.AddHostedService<MixDbCommandSubscriber>();

            services.AddMixQuartzServices(configuration);
            services.AddHostedService<StorageBackgroundTaskSubscriber>();

            if (!globalConfigs!.IsInit)
            {
                services.TryAddSingleton<IAuditLogService, AuditLogService>();
                services.TryAddScoped<AuditLogDataModel>();
                services.AddHostedService<MixLogPublisher>();
            }

            services.AddMixRateLimiter(configuration);
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
            var globalConfigs = configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>()!;
            if (!globalConfigs!.IsInit)
            {
                app.UseMiddleware<AuditlogMiddleware>();
            }
            app.UseMixRateLimiter();
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
            endpoints.UseMixMVCEndpoints();
        }
    }
}
