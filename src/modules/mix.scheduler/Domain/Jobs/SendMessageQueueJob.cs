using Mix.Mq.Lib.Models;
using Mix.Quartz.Jobs;
using Mix.Queue.Interfaces;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Mix.Scheduler.Domain.Jobs
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
            var objData = JObject.Parse(context.Trigger.JobDataMap.GetString("data") ?? "{}");
            int tenantId = objData.Value<int>("tenantId");

            var msg = new MessageQueueModel(tenantId)
            {
                Success = true,
                TopicId = objData.Value<string>("topic"),
                Action = objData.Value<string>("action"),
                Data = objData.Value<JObject>("data").ToString()
            };

            QueueService.PushQueue(msg);

            return Task.CompletedTask;
        }
    }
}