using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Mixdb.Publishers;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
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
            IPooledObjectPolicy<RabbitMQ.Client.IModel>? rabbitMQObjectPolicy = null)
            : base(topicId, queueService, configuration, mixEndpointService, logger, rabbitMQObjectPolicy)
        {
        }
    }
}
