using Microsoft.AspNetCore.Http;
using Mix.Shared.Models;

namespace Mix.Service.Interfaces
{
    public interface IAuditLogService
    {
        public void SaveToDatabase(string createdBy, ParsedRequestModel request, bool isSucceed, Exception exception);

        public void LogRequest(HttpContext context);

        public void LogRequest(string createdBy, ParsedRequestModel request);
    }
}
