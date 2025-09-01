using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Compliance;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Services.Compliance;

namespace Mix.Lib.Services.Compliance
{
    public interface IComplianceSeedingService
    {
        Task SeedDefaultPurposes(int tenantId);
        Task SeedDefaultRetentionPolicies(int tenantId);
        Task SeedDefaultClassifications(int tenantId);
        Task SeedAllDefaults(int tenantId);
    }

    public class ComplianceSeedingService : IComplianceSeedingService
    {
        private readonly DatabaseService _databaseService;
        private readonly IDataClassificationService _classificationService;

        public ComplianceSeedingService(DatabaseService databaseService, IDataClassificationService classificationService)
        {
            _databaseService = databaseService;
            _classificationService = classificationService;
        }

        public async Task SeedDefaultPurposes(int tenantId)
        {
            using var context = _databaseService.GetDbContext();
            
            var defaultPurposes = new[]
            {
                new Purpose 
                { 
                    TenantId = tenantId, 
                    Name = "Account Management", 
                    Description = "Managing user accounts and authentication",
                    LawfulBasis = LawfulBasisType.Contract,
                    IsActive = true,
                    DisplayName = "Account Management",
                    Status = Mix.Heart.Enums.MixContentStatus.Published
                },
                new Purpose 
                { 
                    TenantId = tenantId, 
                    Name = "Marketing Communications", 
                    Description = "Sending marketing emails and promotional content",
                    LawfulBasis = LawfulBasisType.Consent,
                    IsActive = true,
                    DisplayName = "Marketing Communications",
                    Status = Mix.Heart.Enums.MixContentStatus.Published
                },
                new Purpose 
                { 
                    TenantId = tenantId, 
                    Name = "Analytics and Improvement", 
                    Description = "Analyzing usage patterns to improve services",
                    LawfulBasis = LawfulBasisType.LegitimateInterests,
                    IsActive = true,
                    DisplayName = "Analytics and Improvement",
                    Status = Mix.Heart.Enums.MixContentStatus.Published
                },
                new Purpose 
                { 
                    TenantId = tenantId, 
                    Name = "Legal Compliance", 
                    Description = "Meeting legal and regulatory requirements",
                    LawfulBasis = LawfulBasisType.LegalObligation,
                    IsActive = true,
                    DisplayName = "Legal Compliance",
                    Status = Mix.Heart.Enums.MixContentStatus.Published
                },
                new Purpose 
                { 
                    TenantId = tenantId, 
                    Name = "Customer Support", 
                    Description = "Providing customer support and assistance",
                    LawfulBasis = LawfulBasisType.Contract,
                    IsActive = true,
                    DisplayName = "Customer Support",
                    Status = Mix.Heart.Enums.MixContentStatus.Published
                }
            };

            foreach (var purpose in defaultPurposes)
            {
                var exists = await context.Set<Purpose>()
                    .AnyAsync(p => p.TenantId == tenantId && p.Name == purpose.Name);
                
                if (!exists)
                {
                    context.Add(purpose);
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task SeedDefaultRetentionPolicies(int tenantId)
        {
            using var context = _databaseService.GetDbContext();
            
            var defaultPolicies = new[]
            {
                new RetentionPolicy 
                { 
                    TenantId = tenantId, 
                    Name = "Audit Log Retention", 
                    Category = "AuditLog",
                    MaxAgeDays = 2555, // 7 years for HIPAA
                    ActionOnExpiry = RetentionAction.Archive,
                    IsActive = true,
                    DisplayName = "Audit Log Retention (7 years)",
                    Description = "HIPAA compliant audit log retention",
                    Status = Mix.Heart.Enums.MixContentStatus.Published
                },
                new RetentionPolicy 
                { 
                    TenantId = tenantId, 
                    Name = "Consent Records", 
                    Category = "ConsentEvent",
                    MaxAgeDays = 1095, // 3 years
                    ActionOnExpiry = RetentionAction.Archive,
                    IsActive = true,
                    DisplayName = "Consent Records (3 years)",
                    Description = "GDPR compliant consent record retention",
                    Status = Mix.Heart.Enums.MixContentStatus.Published
                },
                new RetentionPolicy 
                { 
                    TenantId = tenantId, 
                    Name = "User Data - Inactive", 
                    Category = "UserData",
                    MaxAgeDays = 1095, // 3 years
                    ActionOnExpiry = RetentionAction.Pseudonymize,
                    IsActive = true,
                    DisplayName = "Inactive User Data (3 years)",
                    Description = "Pseudonymize inactive user data after 3 years",
                    Status = Mix.Heart.Enums.MixContentStatus.Published
                },
                new RetentionPolicy 
                { 
                    TenantId = tenantId, 
                    Name = "DSR Request Logs", 
                    Category = "DsrRequest",
                    MaxAgeDays = 2190, // 6 years
                    ActionOnExpiry = RetentionAction.Archive,
                    IsActive = true,
                    DisplayName = "DSR Request Logs (6 years)",
                    Description = "Archive DSR request records for compliance",
                    Status = Mix.Heart.Enums.MixContentStatus.Published
                }
            };

            foreach (var policy in defaultPolicies)
            {
                var exists = await context.Set<RetentionPolicy>()
                    .AnyAsync(p => p.TenantId == tenantId && p.Name == policy.Name);
                
                if (!exists)
                {
                    context.Add(policy);
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task SeedDefaultClassifications(int tenantId)
        {
            await _classificationService.SeedDefaultClassifications(tenantId);
        }

        public async Task SeedAllDefaults(int tenantId)
        {
            await SeedDefaultPurposes(tenantId);
            await SeedDefaultRetentionPolicies(tenantId);
            await SeedDefaultClassifications(tenantId);
        }
    }
}