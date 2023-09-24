using Mix.Lib.Interfaces;
using Mix.Service.Interfaces;

namespace Mix.SignalR.Hubs
{
    public class LogStreamHub : BaseSignalRHub
    {
        public LogStreamHub(IAuditLogService auditLogService, IMixTenantService mixTenantService)
            : base(auditLogService, mixTenantService)
        {
        }
    }
}