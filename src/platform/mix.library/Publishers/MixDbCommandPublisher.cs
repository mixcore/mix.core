using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;

namespace Mix.Lib.Publishers
{
    public class MixDbCommandPublisher : PublisherBase
    {
        private const string TopicId = MixQueueTopics.MixDbCommand;

        public MixDbCommandPublisher(
            IQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService,
            ILogger<MixDbCommandPublisher> logger)
            : base(TopicId, queueService, configuration, mixEndpointService, logger)
        {
        }
    }
}