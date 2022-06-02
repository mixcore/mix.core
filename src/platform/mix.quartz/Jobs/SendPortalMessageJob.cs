using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.SignalR.Enums;
using Mix.SignalR.Models;
using Mix.SignalR.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
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
            if (context.Trigger.JobDataMap.TryGetValue("data", out object obj))
            {
                var msg = JObject.Parse(obj.ToString()).ToObject<SignalRMessageModel>();
                await _portalHub.SendMessageAsync(msg);
            }
        }

        private MessageType GetHubMessageType(JObject obj)
        {
            var status = obj.Value<string>("status");
            switch (status)
            {
                case "success":
                    return MessageType.Success;
                case "error":
                    return MessageType.Error;
                default:
                    return MessageType.Info;
            }
        }
    }
}