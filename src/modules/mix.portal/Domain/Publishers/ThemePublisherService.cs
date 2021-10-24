using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Publishers;
using Mix.Lib.ViewModels;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;

namespace Mix.Portal.Publishers
{
    public class ThemePublisherService : PublisherService
    {
        static string topicId = typeof(MixThemeViewModel).FullName;
        public ThemePublisherService(
            IQueueService<MessageQueueModel> queueService, 
            IConfiguration configuration, IWebHostEnvironment environment,
            MixMemoryMessageQueue<MessageQueueModel> queueMessage) 
            : base(topicId, queueService, configuration, environment, queueMessage)
        {
        }
    }
}
