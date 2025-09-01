using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Compliance;
using Mix.Database.Services.MixGlobalSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Lib.Services.Compliance
{
    public class DpiaService : IDpiaService
    {
        private readonly DatabaseService _databaseService;

        public DpiaService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<DataProtectionImpactAssessment> CreateAssessment(int tenantId, DataProtectionImpactAssessment assessment)
        {
            using var context = _databaseService.GetDbContext();
            
            assessment.TenantId = tenantId;
            assessment.Status = "Draft";
            assessment.CreatedDateTime = DateTime.UtcNow;
            assessment.LastModified = DateTime.UtcNow;

            context.Add(assessment);
            await context.SaveChangesAsync();
            return assessment;
        }

        public async Task<DataProtectionImpactAssessment> UpdateAssessment(int tenantId, int assessmentId, DataProtectionImpactAssessment assessment)
        {
            using var context = _databaseService.GetDbContext();
            
            var existingAssessment = await context.Set<DataProtectionImpactAssessment>()
                .FirstOrDefaultAsync(x => x.Id == assessmentId && x.TenantId == tenantId);

            if (existingAssessment == null)
                throw new InvalidOperationException("DPIA assessment not found");

            if (existingAssessment.Status == "Approved")
                throw new InvalidOperationException("Cannot modify approved DPIA assessment");

            // Update fields
            existingAssessment.Title = assessment.Title;
            existingAssessment.ProcessingDescription = assessment.ProcessingDescription;
            existingAssessment.DataTypes = assessment.DataTypes;
            existingAssessment.DataSubjects = assessment.DataSubjects;
            existingAssessment.LegalBasis = assessment.LegalBasis;
            existingAssessment.RiskScore = assessment.RiskScore;
            existingAssessment.RiskAssessment = assessment.RiskAssessment;
            existingAssessment.Safeguards = assessment.Safeguards;
            existingAssessment.LastModified = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return existingAssessment;
        }

        public async Task<DataProtectionImpactAssessment?> GetAssessment(int tenantId, int assessmentId)
        {
            using var context = _databaseService.GetDbContext();
            
            return await context.Set<DataProtectionImpactAssessment>()
                .Include(x => x.Risks)
                    .ThenInclude(r => r.Mitigations)
                .FirstOrDefaultAsync(x => x.Id == assessmentId && x.TenantId == tenantId);
        }

        public async Task<IEnumerable<DataProtectionImpactAssessment>> GetAssessments(int tenantId, string? status = null)
        {
            using var context = _databaseService.GetDbContext();
            
            var query = context.Set<DataProtectionImpactAssessment>()
                .Where(x => x.TenantId == tenantId);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(x => x.Status == status);

            return await query
                .Include(x => x.Risks)
                .OrderByDescending(x => x.CreatedDateTime)
                .ToListAsync();
        }

        public async Task<DataProtectionImpactAssessment> ApproveAssessment(int tenantId, int assessmentId, string approvedBy)
        {
            using var context = _databaseService.GetDbContext();
            
            var assessment = await context.Set<DataProtectionImpactAssessment>()
                .FirstOrDefaultAsync(x => x.Id == assessmentId && x.TenantId == tenantId);

            if (assessment == null)
                throw new InvalidOperationException("DPIA assessment not found");

            if (assessment.Status == "Approved")
                throw new InvalidOperationException("DPIA assessment is already approved");

            assessment.Status = "Approved";
            assessment.ApprovedBy = approvedBy;
            assessment.ApprovedAt = DateTime.UtcNow;
            assessment.LastModified = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return assessment;
        }

        public async Task<DataProtectionImpactAssessment> RejectAssessment(int tenantId, int assessmentId, string rejectedBy, string rejectionReason)
        {
            using var context = _databaseService.GetDbContext();
            
            var assessment = await context.Set<DataProtectionImpactAssessment>()
                .FirstOrDefaultAsync(x => x.Id == assessmentId && x.TenantId == tenantId);

            if (assessment == null)
                throw new InvalidOperationException("DPIA assessment not found");

            assessment.Status = "Rejected";
            assessment.RejectedBy = rejectedBy;
            assessment.RejectedAt = DateTime.UtcNow;
            assessment.RejectionReason = rejectionReason;
            assessment.LastModified = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return assessment;
        }

        public async Task AddRisk(int tenantId, int assessmentId, DpiaRisk risk)
        {
            using var context = _databaseService.GetDbContext();
            
            var assessment = await context.Set<DataProtectionImpactAssessment>()
                .FirstOrDefaultAsync(x => x.Id == assessmentId && x.TenantId == tenantId);

            if (assessment == null)
                throw new InvalidOperationException("DPIA assessment not found");

            risk.DpiaId = assessmentId;
            risk.IdentifiedAt = DateTime.UtcNow;

            context.Add(risk);
            await context.SaveChangesAsync();
        }

        public async Task AddMitigation(int tenantId, int riskId, DpiaMitigation mitigation)
        {
            using var context = _databaseService.GetDbContext();
            
            var risk = await context.Set<DpiaRisk>()
                .Include(r => r.Dpia)
                .FirstOrDefaultAsync(x => x.Id == riskId && x.Dpia!.TenantId == tenantId);

            if (risk == null)
                throw new InvalidOperationException("DPIA risk not found");

            mitigation.RiskId = riskId;
            mitigation.ImplementedAt = DateTime.UtcNow;

            context.Add(mitigation);
            await context.SaveChangesAsync();
        }

        public async Task<bool> RequiresDpia(string dataTypes, string processingType, int riskScore)
        {
            // GDPR Article 35 criteria for mandatory DPIA
            var highRiskDataTypes = new[] { "biometric", "genetic", "health", "racial", "political", "religious", "trade union", "sex life", "criminal" };
            var highRiskProcessing = new[] { "systematic monitoring", "large scale", "automated decision making", "profiling", "special categories", "vulnerable subjects" };

            var containsHighRiskData = highRiskDataTypes.Any(type => 
                dataTypes.ToLowerInvariant().Contains(type.ToLowerInvariant()));

            var containsHighRiskProcessing = highRiskProcessing.Any(type => 
                processingType.ToLowerInvariant().Contains(type.ToLowerInvariant()));

            // Mandatory DPIA if:
            // 1. High risk score (8+ out of 10)
            // 2. Processing special categories of data
            // 3. High-risk processing activities
            return riskScore >= 8 || containsHighRiskData || containsHighRiskProcessing;
        }

        public async Task<DpiaReport> GenerateComplianceReport(int tenantId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            using var context = _databaseService.GetDbContext();
            
            fromDate ??= DateTime.UtcNow.AddDays(-90);
            toDate ??= DateTime.UtcNow;

            var assessments = await context.Set<DataProtectionImpactAssessment>()
                .Where(x => x.TenantId == tenantId && 
                           x.CreatedDateTime >= fromDate && 
                           x.CreatedDateTime <= toDate)
                .Include(x => x.Risks)
                .ToListAsync();

            var risks = assessments.SelectMany(a => a.Risks ?? new List<DpiaRisk>()).ToList();

            var report = new DpiaReport
            {
                TenantId = tenantId,
                TotalAssessments = assessments.Count,
                PendingAssessments = assessments.Count(x => x.Status == "Pending" || x.Status == "Draft"),
                ApprovedAssessments = assessments.Count(x => x.Status == "Approved"),
                RejectedAssessments = assessments.Count(x => x.Status == "Rejected"),
                OverdueAssessments = assessments.Count(x => x.Status != "Approved" && 
                    x.CreatedDateTime < DateTime.UtcNow.AddDays(-30)),
                HighRiskAssessments = assessments.Count(x => x.RiskScore >= 8),
                AssessmentsByStatus = assessments.GroupBy(x => x.Status)
                    .ToDictionary(g => g.Key ?? "Unknown", g => g.Count()),
                RisksByCategory = risks.GroupBy(x => x.Category)
                    .ToDictionary(g => g.Key.ToString(), g => g.Count()),
                AverageRiskScore = assessments.Any() ? assessments.Average(x => x.RiskScore) : 0,
                GeneratedAt = DateTime.UtcNow,
                ReportPeriodStart = fromDate.Value,
                ReportPeriodEnd = toDate.Value
            };

            return report;
        }

        public async Task<IEnumerable<DataProtectionImpactAssessment>> GetOverdueAssessments(int tenantId)
        {
            using var context = _databaseService.GetDbContext();
            
            var cutoffDate = DateTime.UtcNow.AddDays(-30);
            
            return await context.Set<DataProtectionImpactAssessment>()
                .Where(x => x.TenantId == tenantId && 
                           (x.Status == "Pending" || x.Status == "Draft") &&
                           x.CreatedDateTime < cutoffDate)
                .OrderBy(x => x.CreatedDateTime)
                .ToListAsync();
        }

        public async Task<DataProtectionImpactAssessment> ReviewAssessment(int tenantId, int assessmentId, string reviewComments)
        {
            using var context = _databaseService.GetDbContext();
            
            var assessment = await context.Set<DataProtectionImpactAssessment>()
                .FirstOrDefaultAsync(x => x.Id == assessmentId && x.TenantId == tenantId);

            if (assessment == null)
                throw new InvalidOperationException("DPIA assessment not found");

            assessment.Status = "Under Review";
            assessment.ReviewComments = reviewComments;
            assessment.ReviewedAt = DateTime.UtcNow;
            assessment.LastModified = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return assessment;
        }
    }
}