using Mix.Lib.Interfaces;

namespace Mix.SignalR.Hubs
{
    public class EditFileHub : BaseSignalRHub
    {
        public EditFileHub(IAuditLogService auditLogService, IMixTenantService mixTenantService) 
            : base(auditLogService, mixTenantService)
        {
        }
    }
}