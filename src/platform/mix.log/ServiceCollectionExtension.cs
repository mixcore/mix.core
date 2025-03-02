using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Mix.Constant.Constants;
using Mix.Database.Entities.AuditLog;
using Mix.Database.Entities.QueueLog;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Log.Lib.Interfaces;
using Mix.Log.Lib.Models;
using Mix.Log.Lib.Publishers;
using Mix.Log.Lib.Services;
using Mix.Log.Lib.Subscribers;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Queue.Services;
using Mix.Service.Services;
using Mix.Shared.Models.Configurations;
using Mix.SignalR.Interfaces;

namespace Mix.Log.Lib
{
    public static class ServiceCollectionExtensions
    {
        // Only need to add these services to the main service 
        // which is use for subscribe and store log messages
        // For other application:
        //      builder.Services.AddHostedService<MixLogPublisher>();
        //      app.UseMiddleware<AuditlogMiddleware>();
        // For Ex: store log message to db, just need to add these services to main application only
        public static IServiceCollection AddMixLogSubscriber(this IServiceCollection services, IConfiguration configuration)
        {
            var globalConfigs = configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>()!;

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<DatabaseService>();
            services.TryAddSingleton<IMemoryQueueService<MessageQueueModel>, MemoryQueueService>();
            services.TryAddSingleton<ILogStreamHubClientService, LogStreamHubClientService>();
            services.TryAddSingleton<IPortalHubClientService, PortalHubClientService>();
            services.TryAddSingleton<IMixTenantService, MixTenantService>();


            services.AddDbContext<AuditLogDbContext>();
            services.AddDbContext<QueueLogDbContext>();
            
            services.TryAddSingleton<IAuditLogService, AuditLogService>();
            if (!configuration.IsInit())
            {
                services.TryAddSingleton<IPortalHubClientService, PortalHubClientService>();
                services.TryAddSingleton<IMixQueueLog, MixQueueLogService>();
                services.AddHostedService<MixLogSubscriber>();
            }
            return services;
        }

        public static IHostApplicationBuilder AddMixLogPublisher(this IHostApplicationBuilder builder)
        {
            builder.Services.TryAddSingleton<IAuditLogService, AuditLogService>();
            builder.Services.TryAddScoped<AuditLogDataModel>();
            builder.Services.AddHostedService<MixLogPublisher>();

            return builder;
        }
    }
}
