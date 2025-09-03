using Mix.Database.Entities.Compliance;

namespace Mix.Lib.Services.Compliance
{
    public interface IBreakGlassService
    {
        Task<BreakGlassAccess> RequestEmergencyAccess(int tenantId, Guid userId, string reason, string justification, int durationMinutes = 30);
        Task<BreakGlassAccess> ApproveAccess(int tenantId, int accessId, string approvedBy);
        Task<BreakGlassAccess> RevokeAccess(int tenantId, int accessId, string revokedBy);
        Task<bool> HasActiveBreakGlassAccess(int tenantId, Guid userId);
        Task<IEnumerable<BreakGlassAccess>> GetActiveAccesses(int tenantId);
        Task LogBreakGlassAction(int accessId, string action, string entityType, string entityId, bool phiAccessed);
        Task<BreakGlassMetrics> GetBreakGlassMetrics(int tenantId, DateTime? fromDate = null, DateTime? toDate = null);
    }

    public class BreakGlassMetrics
    {
        public int TotalRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int ActiveSessions { get; set; }
        public int PhiAccessEvents { get; set; }
        public Dictionary<string, int> RequestsByReason { get; set; }
        public double AverageSessionDuration { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}