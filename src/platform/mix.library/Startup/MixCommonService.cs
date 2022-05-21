using Microsoft.Extensions.Configuration;
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
            services.AddSingleton<HttpService>();
            services.AddSingleton<MixDatabaseService>();
            services.AddSingleton<AuditLogService>();
            services.AddHostedService<MixBackgrouTaskPublisher>();
            services.AddHostedService<MixBackgrouTaskSubscriber>();

            services.AddScoped<MixHeartConfigService>();
            services.AddScoped<CultureService>();
            services.AddScoped<AuthConfigService>();
            services.AddScoped<SmtpConfigService>();
            services.AddScoped<MixEndpointService>();
            services.AddScoped<IPSecurityConfigService>();
            services.AddScoped<MixPostService>();
            services.AddScoped<MixDataService>();

            services.AddScoped<MixService>();
            services.AddScoped<TranslatorService>();
            services.AddScoped<MixConfigurationService>();
            return services;
        }
    }
}
