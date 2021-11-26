using Mix.Lib.Publishers;
using Mix.Lib.ViewModels;
using Mix.Queue.Engines.MixQueue;

namespace Mix.Portal.Publishers
{
    public class TemplatePublisherService : PublisherService
    {
        static string topicId = typeof(MixTemplateViewModel).FullName;
        public TemplatePublisherService(
            IQueueService<MessageQueueModel> queueService, 
            IConfiguration configuration, IWebHostEnvironment environment,
            MixMemoryMessageQueue<MessageQueueModel> queueMessage) 
            : base(topicId, queueService, configuration, environment, queueMessage)
        {
        }
    }
}
