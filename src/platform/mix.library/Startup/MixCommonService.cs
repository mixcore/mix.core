using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Services;
using Mix.Lib.Publishers;
using Mix.Lib.Services;
using Mix.Lib.Subscribers;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static IServiceCollection AddMixCommonServices(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {
            services.TryAddSingleton<HttpService>();
            services.TryAddSingleton<DatabaseService>();
            services.TryAddSingleton<RuntimeDbContextService>();
            services.TryAddSingleton<AuditLogService>();
            services.AddHostedService<MixBackgrouTaskPublisher>();
            services.AddHostedService<MixBackgrouTaskSubscriber>();

            services.TryAddScoped<MixHeartConfigService>();
            services.TryAddSingleton<CultureService>();
            services.TryAddScoped<AuthConfigService>();
            services.TryAddScoped<SmtpConfigService>();
            services.TryAddScoped<MixEndpointService>();
            services.TryAddScoped<IPSecurityConfigService>();
            services.TryAddScoped<MixPostService>();
            services.TryAddScoped<MixDataService>();

            services.TryAddScoped<MixService>();
            services.TryAddScoped<TranslatorService>();
            services.TryAddScoped<MixConfigurationService>();
            return services;
        }
    }
}
