using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mix.SignalR;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixSignalR(this IServiceCollection services, IConfiguration configuration)
        {
            string azureConnectionString = configuration.GetSection("Azure")["SignalRConnectionString"];

            services
                .AddSingleton<HubFilter>()
                .AddSignalR(options =>
                {
                    options.EnableDetailedErrors = true;
                    options.AddFilter<HubFilter>();
                })
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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

    public class HubFilter(ILogger<HubFilter> logger) : IHubFilter
    {
        private readonly ILogger<HubFilter> _logger = logger;

        public async ValueTask<object> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            _logger.LogInformation("Calling hub method {HubMethodName}", invocationContext.HubMethodName);
            try
            {
                return await next(invocationContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Calling hub method {HubMethodName}", invocationContext.HubMethodName);
                throw;
            }
        }

        public Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
        {
            _logger.LogInformation("Hub connected");
            try
            {
                return next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurs while connecting hub");
                throw;
            }
        }

        public Task OnDisconnectedAsync(HubLifetimeContext context, Exception exception, Func<HubLifetimeContext, Exception, Task> next)
        {
            _logger.LogInformation("Hub disconnected");
            try
            {
                return next(context, exception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurs while disconnecting hub");
                throw;
            }
        }
    }
}
