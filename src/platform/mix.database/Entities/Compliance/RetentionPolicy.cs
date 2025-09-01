using Mix.Database.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mix.Database.Entities.Compliance
{
    public class RetentionPolicy : TenantEntityBase<int>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;
        
        public int MaxAgeDays { get; set; }
        
        [Required]
        public RetentionAction ActionOnExpiry { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public virtual ICollection<DataFieldMetadata>? DataFields { get; set; }
        public virtual ICollection<RetentionExecution>? RetentionExecutions { get; set; }
    }
    
    public enum RetentionAction
    {
        Delete = 1,
        Pseudonymize = 2,
        Archive = 3
    }
}