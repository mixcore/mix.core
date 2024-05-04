using Mix.Lib.Interfaces;

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