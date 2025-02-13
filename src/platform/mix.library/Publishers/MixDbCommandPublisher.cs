using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using RabbitMQ.Client;

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
            IPooledObjectPolicy<RabbitMQ.Client.IModel>? rabbitMQObjectPolicy = null)
            : base(TopicId, queueService, configuration, mixEndpointService, logger, rabbitMQObjectPolicy)
        {
        }
    }
}