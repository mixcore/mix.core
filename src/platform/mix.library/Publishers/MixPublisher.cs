using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.RepoDb.Publishers;
using RabbitMQ.Client;

namespace Mix.Lib.Publishers
{
    // Use for instance of ViewModelBase
    public class MixPublisher<T> : PublisherBase
    {
        static string topicId = typeof(T).FullName;
        public MixPublisher(
            IMemoryQueueService<MessageQueueModel> queueService,
            IConfiguration configuration, IWebHostEnvironment environment,
            MixEndpointService mixEndpointService,
            ILogger<MixRepoDbPublisher> logger,
            IPooledObjectPolicy<IModel> rabbitMqObjectPolicy = null)
            : base(topicId, queueService, configuration, mixEndpointService, logger, rabbitMqObjectPolicy)
        {
        }
    }
}
