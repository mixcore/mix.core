using Mix.Heart.Helpers;
using Mix.Service.Models;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;

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
