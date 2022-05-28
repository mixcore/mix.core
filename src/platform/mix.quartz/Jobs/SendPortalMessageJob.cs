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
            string objData = context.Trigger.JobDataMap.GetString("data") ?? "{}";
            var obj = JObject.Parse(objData);
            SignalRMessageModel<JObject> msg = new()
            {
                Title = "Message from SendPortalMessageJob",
                Data = obj,
                Type = GetHubMessageType(obj)
            };
            await _portalHub.SendMessageAsync(msg.ToString());
        }

        private HubMessageType GetHubMessageType(JObject obj)
        {
            var status = obj.Value<string>("status");
            switch (status)
            {
                case "success":
                    return HubMessageType.Success;
                case "error":
                    return HubMessageType.Error;
                default:
                    return HubMessageType.Success;
            }
        }
    }
}