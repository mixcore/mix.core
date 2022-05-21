using Microsoft.Extensions.Configuration;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mix.Lib.Publishers
{
    public class MixBackgrouTaskPublisher : PublisherBase
    {
        static string topicId = MixQueueTopics.MixBackgroundTasks;
        public MixBackgrouTaskPublisher(
            IQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueMessage)
            : base(topicId, queueService, configuration, queueMessage)
        {
        }
    }
}