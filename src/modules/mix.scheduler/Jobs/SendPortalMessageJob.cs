using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Threading.Tasks;
using Mix.Quartz.Jobs;
using Mix.SignalR.Interfaces;

namespace Mix.Scheduler.Jobs
{
    public class SendPortalMessageJob : MixJobBase
    {
        private readonly IPortalHubClientService _portalHub;
        public SendPortalMessageJob(
            IServiceProvider serviceProvider,
            IQueueService<MessageQueueModel> queueService,
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