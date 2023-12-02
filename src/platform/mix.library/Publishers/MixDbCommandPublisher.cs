using Microsoft.Extensions.Configuration;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mix.Lib.Publishers
{
    public class MixDbCommandPublisher : PublisherBase
    {
        private const string TopicId = MixQueueTopics.MixDbCommand;

        public MixDbCommandPublisher(
            IQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixQueueMessages<MessageQueueModel> queueMessage,
            MixEndpointService mixEndpointService)
            : base(TopicId, queueService, configuration, queueMessage, mixEndpointService)
        {
        }
    }
}