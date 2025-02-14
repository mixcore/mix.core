using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Constant.Constants;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;

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
            IPooledObjectPolicy<RabbitMQ.Client.IModel>? rabbitMQObjectPolicy = null)
            : base(TopicId, queueService, configuration, mixEndpointService, logger, rabbitMQObjectPolicy)
        {
        }
    }
}