using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Constant.Constants;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Engines.RabbitMQ;
using Mix.Queue.Interfaces;

namespace Mix.Log.Lib.Publishers
{
    public class MixLogPublisher : PublisherBase
    {
        public MixLogPublisher(
            IMemoryQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService,
            ILogger<MixLogPublisher> logger,
            RabbitModelPooledObjectPolicy? rabbitMQObjectPolicy = null)
            : base(MixQueueTopics.MixLog, queueService, configuration, mixEndpointService, logger, rabbitMQObjectPolicy)
        {
        }
    }
}