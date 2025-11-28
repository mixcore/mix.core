using Mix.Mq.Lib.Models;
using Mix.Quartz.Jobs;
using Mix.Queue.Interfaces;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Mix.Scheduler.Domain.Jobs
{
    public class PublishScheduledPostsJob : MixJobBase
    {
        public PublishScheduledPostsJob(
            IServiceProvider serviceProvider,
            IMemoryQueueService<MessageQueueModel> queueService) : base(serviceProvider, queueService)
        {
        }

        public override Task ExecuteHandler(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}