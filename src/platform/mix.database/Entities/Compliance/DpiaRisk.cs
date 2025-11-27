using Mix.Database.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mix.Database.Entities.Compliance
{
    public class DpiaRisk : TenantEntityBase<int>
    {
        public int DpiaId { get; set; }
        public virtual DataProtectionImpactAssessment? Dpia { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string RiskDescription { get; set; } = string.Empty;
        
        [Required]
        public RiskCategory Category { get; set; }
        
        public int Likelihood { get; set; } // 1-5 scale
        public int Impact { get; set; } // 1-5 scale
        public int InherentRisk => Likelihood * Impact; // Calculated field
        
        [MaxLength(1000)]
        public string? ExistingControls { get; set; }
        
        public int ResidualLikelihood { get; set; }
        public int ResidualImpact { get; set; }
        public int ResidualRisk => ResidualLikelihood * ResidualImpact;
        
        [Required]
        public RiskStatus Status { get; set; }
        
        [MaxLength(100)]
        public string? Owner { get; set; }
        
        public DateTime? TargetDate { get; set; }
        
        public DateTime IdentifiedAt { get; set; }
        
        public virtual ICollection<DpiaMitigation>? Mitigations { get; set; }
    }
    
    public enum RiskCategory
    {
        UnauthorizedAccess = 1,
        DataBreach = 2,
        IdentityTheft = 3,
        Discrimination = 4,
        FinancialLoss = 5,
        ReputationalDamage = 6,
        PsychologicalHarm = 7,
        PhysicalHarm = 8,
        Other = 9
    }
    
    public enum RiskStatus
    {
        Identified = 1,
        Analyzed = 2,
        Mitigated = 3,
        Accepted = 4,
        Transferred = 5,
        Avoided = 6
    }
}