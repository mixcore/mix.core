using Microsoft.AspNetCore.Http;
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
        public static IServiceCollection AddMixCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<HttpService>();
            services.TryAddSingleton<DatabaseService>();
            services.TryAddSingleton<ILogStreamHubClientService, LogStreamHubClientService>();
            services.TryAddSingleton<MixEndpointService>();


            services.TryAddScoped<MixHeartConfigService>();
            services.TryAddScoped<AuthConfigService>();
            services.TryAddScoped<SmtpConfigService>();
            services.TryAddScoped<IPSecurityConfigService>();
            
            services.TryAddSingleton<MixPermissionService>();
            services.TryAddScoped<TranslatorService>();
            
            services.TryAddScoped<EmailService>();
            services.TryAddScoped<IMixEdmService, MixEdmService>();
            
            return services;
        }
    }
}