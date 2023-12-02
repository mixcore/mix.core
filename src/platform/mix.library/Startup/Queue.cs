using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Services;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static IServiceCollection AddQueues(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {

            // Message Queue
            // Need singleton instance to store all message from mix publishers
            services.TryAddSingleton<IQueueService<MessageQueueModel>, QueueService>();
            return services;
        }
    }
}
