using Mix.Lib.Interfaces;
using Mix.Service.Interfaces;

namespace Mix.SignalR.Hubs
{
    public class MixThemeHub : BaseSignalRHub
    {
        public MixThemeHub(IMixTenantService mixTenantService)
            : base(mixTenantService)
        {
        }
    }
}