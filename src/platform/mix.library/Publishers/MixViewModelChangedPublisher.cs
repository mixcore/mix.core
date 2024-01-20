using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.RepoDb.Publishers;

namespace Mix.Lib.Publishers
{
    public class MixViewModelChangedPublisher : PublisherBase
    {
        private const string TopicId = MixQueueTopics.MixViewModelChanged;

        public MixViewModelChangedPublisher(
            IMemoryQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService,
            ILogger<MixViewModelChangedPublisher> logger)
            : base(TopicId, queueService, configuration, mixEndpointService, logger)
        {
        }
    }
}