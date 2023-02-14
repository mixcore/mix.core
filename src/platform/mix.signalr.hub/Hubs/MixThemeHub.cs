using Mix.Service.Interfaces;

namespace Mix.SignalR.Hubs
{
    public class MixThemeHub : BaseSignalRHub
    {
        public MixThemeHub(IAuditLogService auditLogService) : base(auditLogService)
        {
        }
    }
}