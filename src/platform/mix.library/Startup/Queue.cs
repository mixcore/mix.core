using Microsoft.Extensions.Configuration;
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
            services.AddSingleton<IQueueService<MessageQueueModel>, QueueService>();
            // Need singleton instance to store all message from mix publishers (inherit from MixPublisher)
            services.AddSingleton<MixMemoryMessageQueue<MessageQueueModel>>();
            return services;
        }
    }
}
