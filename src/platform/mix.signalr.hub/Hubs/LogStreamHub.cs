using Mix.Lib.Interfaces;
using Mix.Service.Interfaces;

namespace Mix.SignalR.Hubs
{
    public class LogStreamHub : BaseSignalRHub
    {
        public LogStreamHub(IMixTenantService mixTenantService)
            : base(mixTenantService)
        {
        }
    }
}