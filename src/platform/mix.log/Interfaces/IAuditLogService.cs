using Mix.Log.Lib.Models;

namespace Mix.Log.Lib.Interfaces
{
    public interface IAuditLogService
    {
        public Task SaveRequestAsync(AuditLogDataModel request);
        public void QueueRequest(AuditLogDataModel request);
        Task LogStream(string? message, object? data = null, Exception? ex = null, bool isSuccess = false);
    }
}
