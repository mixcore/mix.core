using Mix.Lib.Services;

namespace Mix.SignalR.Hubs
{
    public class MixThemeHub : BaseSignalRHub
    {
        public MixThemeHub(AuditLogService auditLogService) : base(auditLogService)
        {
        }
    }
}