using Microsoft.AspNetCore.Builder;
using System.Reflection;
using Mix.Lib.Services;
using Mix.Database.Services;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static IServiceCollection AddMixCommonServices(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {
            services.AddScoped<MixHeartConfigService>();
            services.AddScoped<MixDatabaseService>();
            services.AddScoped<CultureService>();
            services.AddScoped<AuthConfigService>();
            services.AddScoped<SmtpConfigService>();
            services.AddScoped<MixEndpointService>();
            services.AddScoped<IPSecurityConfigService>();
            services.AddScoped<MixDataService>();
            services.AddSingleton<HttpService>();

            services.AddScoped<MixService>();
            services.AddScoped<TranslatorService>();
            services.AddScoped<MixConfigurationService>();
            return services;
        }
    }
}
