using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Compliance;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Lib.Services.Compliance
{
    public class DataClassificationService : IDataClassificationService
    {
        private readonly DatabaseService _databaseService;

        public DataClassificationService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<DataFieldMetadata> GetFieldMetadata(int tenantId, string entityName, string fieldName)
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
                    Status = Mix.Heart.Enums.MixContentStatus.Published
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

            return new ComplianceReport
            {
                TotalEntities = allClassifications.Select(x => x.EntityName).Distinct().Count(),
                ClassifiedEntities = allClassifications.Count,
                EncryptedFields = allClassifications.Count(x => x.EncryptionRequired),
                ClassificationBreakdown = classificationBreakdown,
                GeneratedAt = DateTime.UtcNow
            };
        }
    }
}