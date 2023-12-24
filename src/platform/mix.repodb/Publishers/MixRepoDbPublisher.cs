using Microsoft.Extensions.Configuration;
using Mix.Constant.Constants;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Services;

namespace Mix.RepoDb.Publishers
{
    public class MixRepoDbPublisher : PublisherBase
    {
        public MixRepoDbPublisher(
            IQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService)
            : base(MixQueueTopics.MixRepoDb, queueService, configuration, mixEndpointService)
        {

        }
    }
}
