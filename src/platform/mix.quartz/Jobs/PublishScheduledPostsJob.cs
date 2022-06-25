using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
{
    public class PublishScheduledPostsJob : MixJobBase
    {
        public PublishScheduledPostsJob(
            IServiceProvider provider,
            IQueueService<MessageQueueModel> queueService)
            : base(provider, queueService, true)
        {
        }

        public override Task ExecuteHandler(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}