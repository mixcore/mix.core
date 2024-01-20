using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;

namespace Mix.Lib.Publishers
{
    public class MixDbCommandPublisher : PublisherBase
    {
        private const string TopicId = MixQueueTopics.MixDbCommand;

        public MixDbCommandPublisher(
            IMemoryQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService,
            ILogger<MixDbCommandPublisher> logger,
            IPooledObjectPolicy<RabbitMQ.Client.IModel> rabbitMqObjectPolicy = null)
            : base(TopicId, queueService, configuration, mixEndpointService, logger, rabbitMqObjectPolicy)
        {
        }
    }
}