using Mix.Log.Models;

namespace Mix.Log.Commands
{
    public class LogAuditLogCommand
    {
        public LogAuditLogCommand(AuditLogDataModel request)
        {
            Request = request;
        }

        public AuditLogDataModel Request { get; set; }
    }
}
