using Mix.MixQuartz.Jobs;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Mix.Scheduler.Jobs
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