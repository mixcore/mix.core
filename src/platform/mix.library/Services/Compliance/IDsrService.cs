using Mix.Database.Entities.Compliance;

namespace Mix.Lib.Services.Compliance
{
    public interface IDsrService
    {
        Task<DsrRequest> SubmitRequest(int tenantId, Guid userId, DsrRequestType requestType, string notes = null);
        Task<DsrRequest> GetRequest(int tenantId, int requestId);
        Task<IEnumerable<DsrRequest>> GetUserRequests(int tenantId, Guid userId);
        Task<IEnumerable<DsrRequest>> GetOverdueRequests(int tenantId);
        Task<DsrRequest> ProcessRequest(int tenantId, int requestId, string processedBy);
        Task<DsrRequest> CompleteRequest(int tenantId, int requestId, string processedBy, string exportFilePath = null);
        Task<DsrRequest> RejectRequest(int tenantId, int requestId, string processedBy, string rejectionReason);
        Task<string> ExportUserData(int tenantId, Guid userId);
        Task<bool> EraseUserData(int tenantId, Guid userId);
        Task<DsrMetrics> GetSlaMetrics(int tenantId, DateTime? fromDate = null, DateTime? toDate = null);
    }

    public class DsrMetrics
    {
        public int TotalRequests { get; set; }
        public int CompletedRequests { get; set; }
        public int OverdueRequests { get; set; }
        public int SlaBreaches { get; set; }
        public double AverageProcessingDays { get; set; }
        public Dictionary<DsrRequestType, int> RequestsByType { get; set; }
        public Dictionary<DsrRequestStatus, int> RequestsByStatus { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}