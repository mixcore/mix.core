using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
{
    public class SendMessageQueueJob : MixJobBase
    {
        readonly IQueueService<MessageQueueModel> _queueService;
        public SendMessageQueueJob(IQueueService<MessageQueueModel> queueService, IServiceProvider provider)
            : base(provider)
        {
            _queueService = queueService;
        }

        public override Task ExecuteHandler(IJobExecutionContext context)
        {
            string objData = context.Trigger.JobDataMap.GetString("data") ?? "{}";
            var msg = new MessageQueueModel()
            {
                Success = true,
                TopicId = context.Trigger.JobDataMap.GetString("topic"),
                Action = context.Trigger.JobDataMap.GetString("action"),
                Data = JObject.Parse(objData)
            };
            _queueService.PushQueue(msg);

            return Task.CompletedTask;
        }
    }
}