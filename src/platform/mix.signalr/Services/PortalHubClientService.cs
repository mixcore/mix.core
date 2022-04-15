using Mix.Shared.Services;
using Mix.SignalR.Constants;

namespace Mix.SignalR.Services
{
    public class PortalHubClientService: BaseHubClientService
    {
        public PortalHubClientService()
            : base(HubEndpoints.PortalHub)
        {
        }
    }
}
