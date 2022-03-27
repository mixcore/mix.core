using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Mix.Queue.Engines.MixQueue;

namespace Mix.Lib.Publishers
{
    public class MixPublisher<T> : PublisherServiceBase
    {
        static string topicId = typeof(T).FullName;
        public MixPublisher(
            IQueueService<MessageQueueModel> queueService,
            IConfiguration configuration, IWebHostEnvironment environment,
            MixMemoryMessageQueue<MessageQueueModel> queueMessage)
            : base(topicId, queueService, configuration, environment, queueMessage)
        {
        }
    }
}
