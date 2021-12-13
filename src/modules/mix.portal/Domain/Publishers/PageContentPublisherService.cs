namespace Mix.Portal.Publishers
{
    public class PageContentPublisherService : MixPublisherServiceBase
    {
        static string topicId = typeof(MixPageContentViewModel).FullName;
        public PageContentPublisherService(
            IQueueService<MessageQueueModel> queueService, 
            IConfiguration configuration, IWebHostEnvironment environment,
            MixMemoryMessageQueue<MessageQueueModel> queueMessage) 
            : base(topicId, queueService, configuration, environment, queueMessage)
        {
        }
    }
}
