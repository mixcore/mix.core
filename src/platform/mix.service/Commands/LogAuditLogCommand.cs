using Mix.Service.Models;

namespace Mix.Service.Commands
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
