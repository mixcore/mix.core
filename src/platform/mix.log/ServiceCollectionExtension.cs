using Google.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Entities.AuditLog;
using Mix.Database.Entities.Queue;
using Mix.Log.Lib.Interfaces;
using Mix.Log.Lib.Models;
using Mix.Log.Lib.Publishers;
using Mix.Log.Lib.Services;
using Mix.Log.Lib.Subscribers;
using Mix.Service.Services;
using Mix.Shared.Interfaces;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;
using Mix.SignalR.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Log.Lib
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixLog(this IServiceCollection services, IConfiguration configuration)
        {
            var globalConfigs = configuration.Get<GlobalConfigurations>();

            services.AddDbContext<AuditLogDbContext>();
            services.AddDbContext<MixQueueDbContext>();
            services.TryAddScoped<AuditLogDataModel>();
            services.TryAddSingleton<IAuditLogService, AuditLogService>();
            
            if (!globalConfigs!.IsInit)
            {
                services.TryAddSingleton<IPortalHubClientService, PortalHubClientService>();
                services.TryAddSingleton<IMixQueueLog, MixQueueLogService>();

                services.AddHostedService<MixLogPublisher>();
                services.AddHostedService<MixLogSubscriber>();
            }
            return services;
        }
    }
}
