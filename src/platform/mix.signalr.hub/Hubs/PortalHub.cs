using Mix.Service.Interfaces;

namespace Mix.SignalR.Hubs
{
    public class PortalHub : BaseSignalRHub
    {
        public PortalHub(IAuditLogService auditLogService) : base(auditLogService)
        {
        }
    }
}