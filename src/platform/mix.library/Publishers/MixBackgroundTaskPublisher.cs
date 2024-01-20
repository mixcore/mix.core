using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;

namespace Mix.Lib.Publishers
{
    public class MixBackgroundTaskPublisher : PublisherBase
    {
        private const string TopicId = MixQueueTopics.MixBackgroundTasks;

        public MixBackgroundTaskPublisher(
            IMemoryQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService,
            ILogger<MixBackgroundTaskPublisher> logger,
            IPooledObjectPolicy<RabbitMQ.Client.IModel> rabbitMqObjectPolicy = null)
            : base(TopicId, queueService, configuration, mixEndpointService, logger, rabbitMqObjectPolicy)
        {
        }
    }
}