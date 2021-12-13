using System.Reflection;
using Microsoft.Extensions.Configuration;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static IServiceCollection AddQueues(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {

            // Message Queue
            services.AddSingleton<IQueueService<MessageQueueModel>, QueueService>();
            services.AddSingleton<MixMemoryMessageQueue<MessageQueueModel>>();
            return services;
        }
    }
}
