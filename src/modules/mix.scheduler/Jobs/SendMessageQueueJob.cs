using Mix.Quartz.Jobs;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Mix.Scheduler.Jobs
{
    public class SendMessageQueueJob : MixJobBase
    {
        public SendMessageQueueJob(
            IQueueService<MessageQueueModel> queueService,
            IServiceProvider serviceProvider) : base(serviceProvider, queueService)
        {
        }

        public override Task ExecuteHandler(IJobExecutionContext context)
        {
            string objData = context.Trigger.JobDataMap.GetString("data") ?? "{}";
            int tenantId = context.Trigger.JobDataMap.GetInt("tenantId");
            var msg = new MessageQueueModel(tenantId)
            {
                Success = true,
                TopicId = context.Trigger.JobDataMap.GetString("topic"),
                Action = context.Trigger.JobDataMap.GetString("action"),
                Data = objData
            };

            QueueService.PushQueue(msg);

            return Task.CompletedTask;
        }
    }
}