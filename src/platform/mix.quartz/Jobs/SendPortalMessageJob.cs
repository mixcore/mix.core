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
        public SendPortalMessageJob(IServiceProvider provider, PortalHubClientService portalHub)
            : base(provider)
        {
            _portalHub = portalHub;
        }

        public override async Task ExecuteHandler(IJobExecutionContext context)
        {
            string objData = context.Trigger.JobDataMap.GetString("data") ?? "{}";

            SignalRMessageModel<JObject> msg = new()
            {
                Message = JObject.Parse(objData),
                Type = SignalR.Enums.HubMessageType.Success
            };
            await _portalHub.SendMessageAsync(msg.ToString());

#if DEBUG
            Console.WriteLine(JObject.FromObject(msg).ToString());
#endif
        }
    }
}