using Mix.Service.Interfaces;

namespace Mix.SignalR.Hubs
{
    public class LogStreamHub : BaseSignalRHub
    {
        public LogStreamHub(IAuditLogService auditLogService) : base(auditLogService)
        {
        }
    }
}