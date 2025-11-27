using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Compliance;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Lib.Services.Compliance
{
    public class DataClassificationService : IDataClassificationService
    {
        private readonly DatabaseService _databaseService;

        public DataClassificationService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<DataFieldMetadata?> GetFieldMetadata(int tenantId, string entityName, string fieldName)
        {
            using var context = _databaseService.GetDbContext();
            return await context.Set<DataFieldMetadata>()
                .Include(x => x.Purpose)
                .Include(x => x.RetentionPolicy)
                .FirstOrDefaultAsync(x => x.TenantId == tenantId 
                    && x.EntityName == entityName 
                    && x.FieldName == fieldName);
        }

        public async Task<IEnumerable<DataFieldMetadata>> GetEntitiesForClassification(int tenantId, DataClassification classification)
        {
            using var context = _databaseService.GetDbContext();
            return await context.Set<DataFieldMetadata>()
                .Include(x => x.Purpose)
                .Include(x => x.RetentionPolicy)
                .Where(x => x.TenantId == tenantId && x.Classification == classification)
                .ToListAsync();
        }

        public async Task<DataFieldMetadata> SetFieldClassification(int tenantId, string entityName, string fieldName, DataClassification classification, bool encryptionRequired = false)
        {
            using var context = _databaseService.GetDbContext();
            
            var existing = await context.Set<DataFieldMetadata>()
                .FirstOrDefaultAsync(x => x.TenantId == tenantId 
                    && x.EntityName == entityName 
                    && x.FieldName == fieldName);

            if (existing != null)
            {
                existing.Classification = classification;
                existing.EncryptionRequired = encryptionRequired;
                existing.LastReviewedUtc = DateTime.UtcNow;
                existing.LastModified = DateTime.UtcNow;
                context.Update(existing);
            }
            else
            {
                existing = new DataFieldMetadata
                {
                    TenantId = tenantId,
                    EntityName = entityName,
                    FieldName = fieldName,
                    Classification = classification,
                    EncryptionRequired = encryptionRequired,
                    LastReviewedUtc = DateTime.UtcNow,
                    DisplayName = $"{entityName}.{fieldName}",
                    CreatedDateTime = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                    Priority = 5
                };
                context.Add(existing);
            }

            await context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> IsFieldClassified(int tenantId, string entityName, string fieldName)
        {
            using var context = _databaseService.GetDbContext();
            return await context.Set<DataFieldMetadata>()
                .AnyAsync(x => x.TenantId == tenantId 
                    && x.EntityName == entityName 
                    && x.FieldName == fieldName);
        }

        public async Task<Dictionary<string, DataClassification>> GetEntityClassifications(int tenantId, string entityName)
        {
            using var context = _databaseService.GetDbContext();
            var fields = await context.Set<DataFieldMetadata>()
                .Where(x => x.TenantId == tenantId && x.EntityName == entityName)
                .ToListAsync();

            return fields.ToDictionary(x => x.FieldName, x => x.Classification);
        }

        public async Task SeedDefaultClassifications(int tenantId)
        {
            // Seed common personal data classifications
            var defaultClassifications = new[]
            {
                new { Entity = "MixUser", Field = "Email", Classification = DataClassification.Personal, Encryption = false },
                new { Entity = "MixUser", Field = "PhoneNumber", Classification = DataClassification.Personal, Encryption = false },
                new { Entity = "MixUser", Field = "UserName", Classification = DataClassification.Personal, Encryption = false },
                new { Entity = "MixUser", Field = "FirstName", Classification = DataClassification.Personal, Encryption = false },
                new { Entity = "MixUser", Field = "LastName", Classification = DataClassification.Personal, Encryption = false },
                new { Entity = "AuditLog", Field = "RequestIp", Classification = DataClassification.Personal, Encryption = false },
                new { Entity = "AuditLog", Field = "UserAgent", Classification = DataClassification.Personal, Encryption = false }
            };

            foreach (var item in defaultClassifications)
            {
                var exists = await IsFieldClassified(tenantId, item.Entity, item.Field);
                if (!exists)
                {
                    await SetFieldClassification(tenantId, item.Entity, item.Field, item.Classification, item.Encryption);
                }
            }
        }

        public async Task<ComplianceReport> GenerateClassificationReport(int tenantId)
        {
            using var context = _databaseService.GetDbContext();
            
            var allClassifications = await context.Set<DataFieldMetadata>()
                .Where(x => x.TenantId == tenantId)
                .ToListAsync();

            var classificationBreakdown = allClassifications
                .GroupBy(x => x.Classification)
                .ToDictionary(g => g.Key, g => g.Count());

            // Get list of entities that might need classification (this would be more sophisticated in real implementation)
            var totalPossibleEntities = new[] { "MixUser", "AuditLog", "ConsentEvent", "DsrRequest" }.Length;

            return new ComplianceReport
            {
                TotalEntities = totalPossibleEntities,
                ClassifiedEntities = allClassifications.Count,
                UnclassifiedEntities = Math.Max(0, totalPossibleEntities - allClassifications.Count),
                EncryptedFields = allClassifications.Count(x => x.EncryptionRequired),
                ClassificationBreakdown = classificationBreakdown,
                UnclassifiedEntityFields = new List<string>(), // Would be populated in real implementation
                GeneratedAt = DateTime.UtcNow
            };
        }
    }
}