using Mix.Lib.Interfaces;

namespace Mix.SignalR.Hubs
{
    public class EditFileHub : BaseSignalRHub
    {
        public EditFileHub(IMixTenantService mixTenantService) 
            : base(mixTenantService)
        {
        }
    }
}