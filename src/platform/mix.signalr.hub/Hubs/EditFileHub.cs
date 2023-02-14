using Mix.Service.Interfaces;

namespace Mix.SignalR.Hubs
{
    public class EditFileHub : BaseSignalRHub
    {
        public EditFileHub(IAuditLogService auditLogService) : base(auditLogService)
        {
        }
    }
}