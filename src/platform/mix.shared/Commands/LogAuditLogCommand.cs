using Mix.Shared.Models;

namespace Mix.Shared.Commands
{
    public class LogAuditLogCommand
    {
        public LogAuditLogCommand()
        {

        }
        public LogAuditLogCommand(Guid id, string userName, ParsedRequestModel request, Exception ex = null)
        {
            LogId = id;
            UserName = userName;
            Request = request;
            Exception = ex;
        } public LogAuditLogCommand(Guid id, int responseStatusCode, Exception ex = null)
        {
            LogId = id;
            StatusCode = responseStatusCode;
            Exception = ex;
        }

        public Guid LogId { get; set; }
        public int StatusCode { get; set; }
        public string UserName { get; set; }
        public ParsedRequestModel Request { get; set; }
        public Exception Exception { get; set; }
    }
}
