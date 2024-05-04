using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Constant.Constants;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Interfaces;
using Mix.Shared.Services;

namespace Mix.RepoDb.Publishers
{
    public class MixRepoDbPublisher : PublisherBase
    {
        public MixRepoDbPublisher(
            IMemoryQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService,
            ILogger<MixRepoDbPublisher> logger,
            IPooledObjectPolicy<RabbitMQ.Client.IModel> rabbitMqObjectPolicy = null)
            : base(MixQueueTopics.MixRepoDb, queueService, configuration, mixEndpointService, logger, rabbitMqObjectPolicy)
        {

        }
    }
}
