using Mix.Lib.Interfaces;
using Mix.Service.Interfaces;

namespace Mix.SignalR.Hubs
{
    public class PortalHub : BaseSignalRHub
    {
        public PortalHub(IMixTenantService mixTenantService)
            : base(mixTenantService)
        {
        }
    }
}