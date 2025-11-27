using Mix.Log.Lib.Models;

namespace Mix.Log.Lib.Commands
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
