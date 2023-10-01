using Mix.Quartz.Jobs;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Mix.Scheduler.Domain.Jobs
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