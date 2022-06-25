using Mix.Lib.Services;

namespace Mix.SignalR.Hubs
{
    public class EditFileHub : BaseSignalRHub
    {
        public EditFileHub(AuditLogService auditLogService) : base(auditLogService)
        {
        }
    }
}