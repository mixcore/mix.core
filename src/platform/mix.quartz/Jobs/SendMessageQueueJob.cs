using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
{
    public class SendMessageQueueJob : MixJobBase
    {
        public SendMessageQueueJob(IQueueService<MessageQueueModel> queueService, IServiceProvider provider)
            : base(provider, queueService)
        {
        }

        public override Task ExecuteHandler(IJobExecutionContext context)
        {
            string objData = context.Trigger.JobDataMap.GetString("data") ?? "{}";
            var msg = new MessageQueueModel()
            {
                Success = true,
                TopicId = context.Trigger.JobDataMap.GetString("topic"),
                Action = context.Trigger.JobDataMap.GetString("action"),
                Data = objData
            };
            _queueService.PushQueue(msg);

            return Task.CompletedTask;
        }
    }
}