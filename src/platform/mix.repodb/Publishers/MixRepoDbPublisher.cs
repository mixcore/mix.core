using Microsoft.Extensions.Configuration;
using Mix.Constant.Constants;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;

namespace Mix.RepoDb.Publishers
{
    public class MixRepoDbPublisher : PublisherBase
    {
        public MixRepoDbPublisher(
            IQueueService<MessageQueueModel> queueService, 
            IConfiguration configuration, 
            MixMemoryMessageQueue<MessageQueueModel> queue) 
            : base(MixQueueTopics.MixRepoDb, queueService, configuration, queue)
        {
            
        }
    }
}
