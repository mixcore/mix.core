using Mix.Database.Entities.Compliance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Lib.Services.Compliance
{
    public interface IConsentService
    {
        Task<ConsentEvent> RecordConsent(int tenantId, Guid userId, int purposeId, bool granted, string method, string? ipAddress, string? userAgent, string version = "1.0");
        Task<bool> HasActiveConsent(int tenantId, Guid userId, int purposeId);
        Task<IEnumerable<ConsentEvent>> GetUserConsentHistory(int tenantId, Guid userId);
        Task<ConsentEvent> WithdrawConsent(int tenantId, Guid userId, int purposeId, string method, string? ipAddress);
        Task<ConsentSummary> GetConsentSummary(int tenantId, Guid userId);
        Task<IEnumerable<ConsentEvent>> GetExpiredConsents(int tenantId, int maxAgeDays);
        Task<ConsentMetrics> GetConsentMetrics(int tenantId, DateTime? fromDate = null, DateTime? toDate = null);
    }

    public class ConsentSummary
    {
        public Guid UserId { get; set; }
        public List<ConsentStatus> Consents { get; set; } = new();
        public DateTime LastUpdated { get; set; }
    }

    public class ConsentStatus
    {
        public int PurposeId { get; set; }
        public string PurposeName { get; set; } = string.Empty;
        public bool Granted { get; set; }
        public DateTime ConsentDate { get; set; }
        public string Method { get; set; } = string.Empty;
    }

    public class ConsentMetrics
    {
        public int TotalConsents { get; set; }
        public int GrantedConsents { get; set; }
        public int WithdrawnConsents { get; set; }
        public Dictionary<int, int> ConsentsByPurpose { get; set; } = new();
        public Dictionary<string, int> ConsentsByMethod { get; set; } = new();
        public DateTime GeneratedAt { get; set; }
    }
}