using Microsoft.AspNetCore.Http;
using Mix.Shared.Models;

namespace Mix.Service.Interfaces
{
    public interface IAuditLogService
    {
        public Task SaveRequestAsync(Guid id, string createdBy, ParsedRequestModel request);
        public Task SaveResponseAsync(Guid id, int statusCode, Exception ex);

        public void LogRequest(Guid id, HttpContext context);

        public void LogRequest(Guid id, string createdBy, ParsedRequestModel request);
        public void LogResponse(Guid id, HttpResponse response, Exception? ex);
    }
}
