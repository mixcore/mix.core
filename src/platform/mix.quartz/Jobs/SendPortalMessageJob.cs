using Microsoft.AspNetCore.SignalR;
using Mix.SignalR.Constants;
using Mix.SignalR.Hubs;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Mix.MixQuartz.Jobs
{
    public class SendPortalMessageJob : MixJobBase
    {
        protected IHubContext<PortalHub> _portalHub;
        public SendPortalMessageJob(IServiceProvider provider, IHubContext<PortalHub> portalHub)
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
            await _portalHub.Clients.All.SendAsync(HubMethods.ReceiveMethod, msg.ToString());

#if DEBUG
            Console.WriteLine(JObject.FromObject(msg).ToString());
#endif
        }
    }
}