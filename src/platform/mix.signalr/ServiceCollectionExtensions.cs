using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixSignalR(this IServiceCollection services, IConfiguration configuration)
        {
            string azureConnectionString = configuration.GetSection("Azure")["SignalRConnectionString"];
            services.AddSignalR()
                   .AddJsonProtocol()
                   .AddAzureSignalRIf(azureConnectionString);
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
