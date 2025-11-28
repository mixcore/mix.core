using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Mix.Automation.Lib.Entities;
using Mix.Automation.Lib.MessageQueue;
using Mix.Automation.Lib.Services;
using Mix.Automation.Lib.Subscribers;
using Mix.Database.Base;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Middlewares;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Queue.Services;
using Mix.Service.Services;
using Mix.Shared.Services;
using Mix.SignalR.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.Extensions
{
    public static class ServiceExtensions
    {
        public static IHostApplicationBuilder AddMixAutomationServices(this IHostApplicationBuilder builder)
        {
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.TryAddSingleton<DatabaseService>();
            builder.Services.AddDbContext<WorkflowDbContext>();
            builder.Services.TryAddScoped<UnitOfWorkInfo<WorkflowDbContext>>();
            builder.Services.TryAddSingleton<IPortalHubClientService, PortalHubClientService>();
            builder.Services.AddMixCache(builder.Configuration);
            builder.Services.TryAddSingleton<HttpService>();
            builder.Services.TryAddScoped<WorkflowService>();
            builder.Services.AddHostedService<MixAutomationPublisher>();
            builder.Services.AddHostedService<MixAutomationSubscriber>();
            builder.Services.TryAddSingleton<IMemoryQueueService<MessageQueueModel>, MemoryQueueService>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<WorkflowDbContext>>();
            return builder;
        }
    }
}
