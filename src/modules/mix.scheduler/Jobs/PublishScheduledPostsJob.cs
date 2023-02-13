using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Quartz;
using System;
using System.Threading.Tasks;
using Mix.Quartz.Jobs;

namespace Mix.Scheduler.Jobs
{
    public class PublishScheduledPostsJob : MixJobBase
    {
        public PublishScheduledPostsJob(
            IServiceProvider serviceProvider,
            IQueueService<MessageQueueModel> queueService) : base(serviceProvider, queueService)
        {
        }

        public override Task ExecuteHandler(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}