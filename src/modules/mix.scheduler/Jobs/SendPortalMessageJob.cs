using Mix.MixQuartz.Jobs;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.SignalR.Models;
using Mix.SignalR.Services;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Mix.Scheduler.Jobs
{
    public class SendPortalMessageJob : MixJobBase
    {
        protected PortalHubClientService _portalHub;
        public SendPortalMessageJob(
            IServiceProvider provider,
            IQueueService<MessageQueueModel> queueService,
            PortalHubClientService portalHub)
            : base(provider, queueService)
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