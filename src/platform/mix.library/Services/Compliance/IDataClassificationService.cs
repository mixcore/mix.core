using Mix.Database.Entities.Compliance;

namespace Mix.Lib.Services.Compliance
{
    public interface IDataClassificationService
    {
        Task<DataFieldMetadata> GetFieldMetadata(int tenantId, string entityName, string fieldName);
        Task<IEnumerable<DataFieldMetadata>> GetEntitiesForClassification(int tenantId, DataClassification classification);
        Task<DataFieldMetadata> SetFieldClassification(int tenantId, string entityName, string fieldName, DataClassification classification, bool encryptionRequired = false);
        Task<bool> IsFieldClassified(int tenantId, string entityName, string fieldName);
        Task<Dictionary<string, DataClassification>> GetEntityClassifications(int tenantId, string entityName);
        Task SeedDefaultClassifications(int tenantId);
        Task<ComplianceReport> GenerateClassificationReport(int tenantId);
    }

    public class ComplianceReport
    {
        public int TotalEntities { get; set; }
        public int ClassifiedEntities { get; set; }
        public int UnclassifiedEntities { get; set; }
        public int EncryptedFields { get; set; }
        public Dictionary<DataClassification, int> ClassificationBreakdown { get; set; }
        public List<string> UnclassifiedEntityFields { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}