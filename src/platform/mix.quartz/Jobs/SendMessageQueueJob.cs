using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
{
    public class SendMessageQueueJob : MixJobBase
    {
        IQueueService<MessageQueueModel> _queueService;
        public SendMessageQueueJob(IQueueService<MessageQueueModel> queueService, IServiceProvider provider)
            : base(provider)
        {
            _queueService = queueService;
        }

        public override Task ExecuteHandler(IJobExecutionContext context)
        {
            var msg = new MessageQueueModel()
            {
                TopicId = context.Trigger.JobDataMap.GetString("topic"),
                Action = context.Trigger.JobDataMap.GetString("action")
            };
            _queueService.PushQueue(msg);
            Console.WriteLine(msg.TopicId);
            Console.WriteLine(msg.Action);
            return Task.CompletedTask;
        }
    }
}