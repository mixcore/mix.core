using Mix.Service.Services;

namespace Mix.SignalR.Hubs
{
    public class PortalHub : BaseSignalRHub
    {
        public PortalHub(AuditLogService auditLogService) : base(auditLogService)
        {
        }
    }
}