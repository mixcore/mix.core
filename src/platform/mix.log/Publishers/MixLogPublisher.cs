using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Constant.Constants;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Shared.Services;

namespace Mix.Log.Lib.Publishers
{
    public class MixLogPublisher : PublisherBase
    {
        private const string TopicId = MixQueueTopics.MixLog;

        public MixLogPublisher(
            IQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService,
            ILogger<MixLogPublisher> logger)
            : base(TopicId, queueService, configuration, mixEndpointService, logger)
        {
        }
    }
}