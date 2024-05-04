using Mix.Mq.Lib.Models;
using Mix.Quartz.Jobs;
using Mix.Queue.Interfaces;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Mix.Scheduler.Domain.Jobs
{
    public class SendPortalMessageJob : MixJobBase
    {
        private readonly IPortalHubClientService _portalHub;
        public SendPortalMessageJob(
            IServiceProvider serviceProvider,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub)
            : base(serviceProvider, queueService)
        {
            _portalHub = portalHub;
        }

        public override async Task ExecuteHandler(IJobExecutionContext context)
        {
            var obj = context.Trigger.JobDataMap.GetString("data");
            if (!string.IsNullOrWhiteSpace(obj))
            {
                var msg = JObject.Parse(obj).ToObject<SignalRMessageModel>();
                msg.From = new(GetType().Name);
                await _portalHub.SendMessageAsync(msg);
            }
        }
    }
}