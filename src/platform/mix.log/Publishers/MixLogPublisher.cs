using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Constant.Constants;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Interfaces;
using Mix.Shared.Services;

namespace Mix.Log.Lib.Publishers
{
    public class MixLogPublisher : PublisherBase
    {
        private const string TopicId = MixQueueTopics.MixLog;

        public MixLogPublisher(
            IMemoryQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService,
            ILogger<MixLogPublisher> logger,
            IPooledObjectPolicy<RabbitMQ.Client.IModel>? rabbitMqObjectPolicy = null)
            : base(TopicId, queueService, configuration, mixEndpointService, logger, rabbitMqObjectPolicy)
        {
        }
    }
}