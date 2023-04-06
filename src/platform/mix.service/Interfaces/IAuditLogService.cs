using Microsoft.AspNetCore.Http;
using Mix.Service.Models;
using Newtonsoft.Json.Linq;

namespace Mix.Service.Interfaces
{
    public interface IAuditLogService
    {
        public Task SaveRequestAsync(AuditLogDataModel request);
        public void QueueRequest(AuditLogDataModel request);
    }
}
