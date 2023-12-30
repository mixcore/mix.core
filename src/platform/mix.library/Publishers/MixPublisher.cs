using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mix.Lib.Publishers
{
    // Use for instance of ViewModelBase
    public class MixPublisher<T> : PublisherBase
    {
        static string topicId = typeof(T).FullName;
        public MixPublisher(
            IQueueService<MessageQueueModel> queueService,
            IConfiguration configuration, IWebHostEnvironment environment,
            MixEndpointService mixEndpointService)
            : base(topicId, queueService, configuration, mixEndpointService)
        {
        }
    }
}
