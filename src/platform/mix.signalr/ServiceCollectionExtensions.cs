using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Mix.SignalR;
using System.Text.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixSignalR(this IServiceCollection services, IConfiguration configuration)
        {
            string azureConnectionString = configuration.GetSection("Azure")["SignalRConnectionString"];
            services.AddSignalR()
                   .AddJsonProtocol(options =>
                   {
                       options.PayloadSerializerOptions.Converters
                          .Add(new JsonStringEnumConverter());
                   })
                   .AddAzureSignalRIf(azureConnectionString);
            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>,
                ConfigureJwtBearerOptions>());

            return services;
        }

        private static void AddAzureSignalRIf(this ISignalRServerBuilder builder, string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                builder.AddAzureSignalR(connectionString);
            }
        }
    }
}
