using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Constant.Constants;
using Mix.Database.Entities.AuditLog;
using Mix.Database.Entities.QueueLog;
using Mix.Database.Services;
using Mix.Lib.Interfaces;
using Mix.Log.Lib.Interfaces;
using Mix.Log.Lib.Models;
using Mix.Log.Lib.Publishers;
using Mix.Log.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Queue.Services;
using Mix.Service.Services;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;
using Mix.SignalR.Interfaces;

namespace Mix.Log.Lib
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixLog(this IServiceCollection services, IConfiguration configuration)
        {
            var globalConfigs = configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>()!;

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<MixEndpointService>();
            services.TryAddSingleton<DatabaseService>();
            services.TryAddSingleton<IMemoryQueueService<MessageQueueModel>, MemoryQueueService>();
            services.TryAddSingleton<ILogStreamHubClientService, LogStreamHubClientService>();
            services.TryAddSingleton<IPortalHubClientService, PortalHubClientService>();
            services.TryAddSingleton<IMixTenantService, MixTenantService>();


            services.AddDbContext<AuditLogDbContext>();
            services.AddDbContext<QueueLogDbContext>();
            services.TryAddScoped<AuditLogDataModel>();
            services.TryAddSingleton<IAuditLogService, AuditLogService>();
            if (!globalConfigs!.IsInit)
            {
                services.AddHostedService<MixLogPublisher>();
                services.TryAddSingleton<IPortalHubClientService, PortalHubClientService>();
                services.TryAddSingleton<IMixQueueLog, MixQueueLogService>();
            }
            return services;
        }
    }
}
