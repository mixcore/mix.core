using Mix.Database.Entities.Compliance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Lib.Services.Compliance
{
    public interface IDpiaService
    {
        Task<DataProtectionImpactAssessment> CreateAssessment(int tenantId, DataProtectionImpactAssessment assessment);
        Task<DataProtectionImpactAssessment> UpdateAssessment(int tenantId, int assessmentId, DataProtectionImpactAssessment assessment);
        Task<DataProtectionImpactAssessment?> GetAssessment(int tenantId, int assessmentId);
        Task<IEnumerable<DataProtectionImpactAssessment>> GetAssessments(int tenantId, string? status = null);
        Task<DataProtectionImpactAssessment> ApproveAssessment(int tenantId, int assessmentId, string approvedBy);
        Task<DataProtectionImpactAssessment> RejectAssessment(int tenantId, int assessmentId, string rejectedBy, string rejectionReason);
        Task AddRisk(int tenantId, int assessmentId, DpiaRisk risk);
        Task AddMitigation(int tenantId, int riskId, DpiaMitigation mitigation);
        Task<bool> RequiresDpia(string dataTypes, string processingType, int riskScore);
        Task<DpiaReport> GenerateComplianceReport(int tenantId, DateTime? fromDate = null, DateTime? toDate = null);
        Task<IEnumerable<DataProtectionImpactAssessment>> GetOverdueAssessments(int tenantId);
        Task<DataProtectionImpactAssessment> ReviewAssessment(int tenantId, int assessmentId, string reviewComments);
    }

    public class DpiaReport
    {
        public int TenantId { get; set; }
        public int TotalAssessments { get; set; }
        public int PendingAssessments { get; set; }
        public int ApprovedAssessments { get; set; }
        public int RejectedAssessments { get; set; }
        public int OverdueAssessments { get; set; }
        public int HighRiskAssessments { get; set; }
        public Dictionary<string, int> AssessmentsByStatus { get; set; } = new();
        public Dictionary<string, int> RisksByCategory { get; set; } = new();
        public double AverageRiskScore { get; set; }
        public DateTime GeneratedAt { get; set; }
        public DateTime ReportPeriodStart { get; set; }
        public DateTime ReportPeriodEnd { get; set; }
    }
}