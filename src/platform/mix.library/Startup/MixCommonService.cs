using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Mix.Communicator.Services;
using Mix.Database.Services;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Interfaces;
using Mix.Lib.Publishers;
using Mix.Lib.Services;
using Mix.Lib.Subscribers;
using Mix.SignalR.Interfaces;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IHostApplicationBuilder AddMixCommonServices(this IHostApplicationBuilder builder)
        {
            builder.Services.TryAddSingleton<AppSettingsService>();
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.TryAddSingleton<HttpService>();
            builder.Services.TryAddSingleton<ILogStreamHubClientService, LogStreamHubClientService>();
            
            builder.Services.TryAddScoped<EmailService>();
            builder.Services.TryAddScoped<IMixEdmService, MixEdmService>();
            
            return builder;
        }
    }
}