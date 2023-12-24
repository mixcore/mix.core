using Microsoft.Extensions.Configuration;
using Mix.Constant.Constants;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Services;

namespace Mix.Log.Lib.Publishers
{
    public class MixLogPublisher : PublisherBase
    {
        private const string TopicId = MixQueueTopics.MixLog;

        public MixLogPublisher(
            IQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService)
            : base(TopicId, queueService, configuration, mixEndpointService)
        {
        }
    }
}