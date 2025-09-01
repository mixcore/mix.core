using Mix.Database.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mix.Database.Entities.Compliance
{
    public class DataFieldMetadata : TenantEntityBase<int>
    {
        [Required]
        [MaxLength(100)]
        public string EntityName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string FieldName { get; set; } = string.Empty;
        
        [Required]
        public DataClassification Classification { get; set; }
        
        public int? PurposeId { get; set; }
        public virtual Purpose? Purpose { get; set; }
        
        public int? RetentionPolicyId { get; set; }
        public virtual RetentionPolicy? RetentionPolicy { get; set; }
        
        public bool EncryptionRequired { get; set; }
        
        public DateTime LastReviewedUtc { get; set; }
        
        [MaxLength(500)]
        public string? Notes { get; set; }
    }
    
    public enum DataClassification
    {
        None = 0,
        Personal = 1,
        Sensitive = 2,
        PHI = 3
    }
}