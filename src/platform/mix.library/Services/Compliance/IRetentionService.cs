namespace Mix.Lib.Services.Compliance
{
    public interface IRetentionService
    {
        Task<RetentionPolicy> CreateRetentionPolicy(int tenantId, string name, string category, int maxAgeDays, RetentionAction action);
        Task<IEnumerable<RetentionPolicy>> GetActiveRetentionPolicies(int tenantId);
        Task<RetentionExecution> ExecuteRetentionPolicy(int tenantId, int policyId, string executedBy);
        Task<IEnumerable<object>> GetExpiredEntities(int tenantId, RetentionPolicy policy);
        Task<bool> AnonymizeEntity(string entityType, object entityId, int tenantId);
        Task<bool> DeleteEntity(string entityType, object entityId, int tenantId);
        Task<RetentionMetrics> GetRetentionMetrics(int tenantId, DateTime? fromDate = null, DateTime? toDate = null);
        Task ScheduleRetentionJobs();
    }

    public class RetentionMetrics
    {
        public int ActivePolicies { get; set; }
        public int TotalExecutions { get; set; }
        public int SuccessfulExecutions { get; set; }
        public int FailedExecutions { get; set; }
        public int ProcessedEntities { get; set; }
        public int DeletedEntities { get; set; }
        public int AnonymizedEntities { get; set; }
        public DateTime LastExecution { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}