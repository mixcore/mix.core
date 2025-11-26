using Castle.Core.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Mix.SignalR;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR.StackExchangeRedis;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IHostApplicationBuilder AddMixSignalR(this IHostApplicationBuilder builder, string? azureConnectionString, string? redisConnection)
        {
            builder.Services
                .AddSignalR(options =>
                {
                    options.EnableDetailedErrors = true;
                    options.MaximumReceiveMessageSize = 15360;
                })
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddBackplaneIf(azureConnectionString, redisConnection);

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>,
                ConfigureJwtBearerOptions>());

            return builder;
        }

        private static void AddBackplaneIf(this ISignalRServerBuilder builder, string connectionString, string redisConnection)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                builder.AddAzureSignalR(connectionString);
            }
            if (!string.IsNullOrEmpty(redisConnection))
            {
                builder.AddStackExchangeRedis(redisConnection);
            }
        }
    }
}
