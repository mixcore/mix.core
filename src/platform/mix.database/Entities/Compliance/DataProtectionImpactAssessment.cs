using Mix.Database.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mix.Database.Entities.Compliance
{
    public class DataProtectionImpactAssessment : TenantEntityBase<int>
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(2000)]
        public string ProcessingDescription { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(1000)]
        public string DataTypes { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(1000)]
        public string DataSubjects { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(1000)]
        public string LegalBasis { get; set; } = string.Empty;
        
        public int RiskScore { get; set; } // 1-10 scale
        
        [MaxLength(2000)]
        public string? RiskAssessment { get; set; }
        
        [MaxLength(2000)]
        public string? Safeguards { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Draft";
        
        [MaxLength(100)]
        public string? ApprovedBy { get; set; }
        
        public DateTime? ApprovedAt { get; set; }
        
        [MaxLength(100)]
        public string? RejectedBy { get; set; }
        
        public DateTime? RejectedAt { get; set; }
        
        [MaxLength(1000)]
        public string? RejectionReason { get; set; }
        
        [MaxLength(2000)]
        public string? ReviewComments { get; set; }
        
        public DateTime? ReviewedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<DpiaRisk>? Risks { get; set; }
    }
    
    public enum DpiaStatus
    {
        Draft = 1,
        UnderReview = 2,
        RequiresChanges = 3,
        Approved = 4,
        Rejected = 5,
        Expired = 6
    }
}