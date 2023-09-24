using Mix.Lib.Interfaces;
using Mix.Service.Interfaces;

namespace Mix.SignalR.Hubs
{
    public class PortalHub : BaseSignalRHub
    {
        public PortalHub(IAuditLogService auditLogService, IMixTenantService mixTenantService)
            : base(auditLogService, mixTenantService)
        {
        }
    }
}