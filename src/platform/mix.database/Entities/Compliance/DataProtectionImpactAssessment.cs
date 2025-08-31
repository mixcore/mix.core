using Mix.Database.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Mix.Database.Entities.Compliance
{
    public class DataProtectionImpactAssessment : TenantEntityBase<int>
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(2000)]
        public string ProcessingDescription { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string DataTypes { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string DataSubjects { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string LegalBasis { get; set; }
        
        public int RiskScore { get; set; } // 1-10 scale
        
        [Required]
        public DpiaStatus Status { get; set; }
        
        [MaxLength(100)]
        public string AssessedBy { get; set; }
        
        public DateTime? AssessmentDate { get; set; }
        
        [MaxLength(100)]
        public string ApprovedBy { get; set; }
        
        public DateTime? ApprovalDate { get; set; }
        
        public DateTime? ReviewDate { get; set; }
        
        [MaxLength(2000)]
        public string Risks { get; set; }
        
        [MaxLength(2000)]
        public string Mitigations { get; set; }
        
        [MaxLength(1000)]
        public string ResidualRisk { get; set; }
        
        public virtual ICollection<DpiaRisk> IdentifiedRisks { get; set; }
        public virtual ICollection<DpiaMitigation> PlannedMitigations { get; set; }
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