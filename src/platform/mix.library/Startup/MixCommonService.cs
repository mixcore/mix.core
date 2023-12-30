using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Communicator.Services;
using Mix.Database.Services;
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
        private static IServiceCollection AddMixCommonServices(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {
            services.TryAddSingleton<HttpService>();
            services.TryAddSingleton<DatabaseService>();
            services.TryAddSingleton<ILogStreamHubClientService, LogStreamHubClientService>();
            services.TryAddSingleton<MixEndpointService>();


            services.TryAddScoped<MixHeartConfigService>();
            services.TryAddScoped<AuthConfigService>();
            services.TryAddScoped<SmtpConfigService>();
            services.TryAddScoped<IPSecurityConfigService>();

            services.TryAddScoped<MixConfigurationService>();
            services.TryAddSingleton<MixPermissionService>();
            services.TryAddScoped<IMixCmsService, MixCmsService>();
            services.TryAddScoped<MixCacheService>();
            services.TryAddScoped<TranslatorService>();

            services.TryAddScoped<EmailService>();
            services.TryAddScoped<IMixEdmService, MixEdmService>();

            services.AddHostedService<MixViewModelChangedPublisher>();
            services.AddHostedService<MixViewModelChangedSubscriber>();

            services.AddHostedService<MixBackgroundTaskPublisher>();
            services.AddHostedService<MixBackgroundTaskSubscriber>();
            services.AddHostedService<MixDbCommandPublisher>();
            services.AddHostedService<MixDbCommandSubscriber>();

            MixPermissionService permissionSrv = services.GetService<MixPermissionService>();
            permissionSrv.Reload().GetAwaiter().GetResult();
            return services;
        }
    }
}