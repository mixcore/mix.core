using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Models;
using System;

namespace Mix.SignalR.Services
{
    public class PortalHubClientService : BaseHubClientService
    {
        public PortalHubClientService(MixEndpointService mixEndpointService)
            : base(HubEndpoints.PortalHub, mixEndpointService)
        {
        }

        protected override void HandleMessage(SignalRMessageModel message)
        {
            Console.WriteLine(message.ToString());
        }
    }
}
