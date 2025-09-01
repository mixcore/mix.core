using Mix.Database.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mix.Database.Entities.Compliance
{
    public class RetentionExecution : TenantEntityBase<int>
    {
        public int RetentionPolicyId { get; set; }
        public virtual RetentionPolicy? RetentionPolicy { get; set; }
        
        public DateTime ExecutedUtc { get; set; }
        
        public int ProcessedCount { get; set; }
        
        public int ErrorCount { get; set; }
        
        [MaxLength(2000)]
        public string? ErrorDetails { get; set; }
        
        public bool Success { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string ExecutedBy { get; set; } = string.Empty;
    }
}