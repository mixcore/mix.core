using Mix.Database.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mix.Database.Entities.Compliance
{
    public class DpiaMitigation : TenantEntityBase<int>
    {
        public int? DpiaId { get; set; }
        public virtual DataProtectionImpactAssessment? Dpia { get; set; }
        
        public int RiskId { get; set; }
        public virtual DpiaRisk? Risk { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string MitigationDescription { get; set; } = string.Empty;
        
        [Required]
        public MitigationType Type { get; set; }
        
        [Required]
        public MitigationStatus Status { get; set; }
        
        [MaxLength(100)]
        public string? Owner { get; set; }
        
        public DateTime? TargetDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime ImplementedAt { get; set; }
        
        [MaxLength(1000)]
        public string? ImplementationNotes { get; set; }
        
        public decimal? EstimatedCost { get; set; }
        
        public int EffectivenessRating { get; set; } // 1-5 scale
    }
    
    public enum MitigationType
    {
        Technical = 1,
        Administrative = 2,
        Physical = 3,
        Legal = 4,
        Organizational = 5
    }
    
    public enum MitigationStatus
    {
        Planned = 1,
        InProgress = 2,
        Completed = 3,
        OnHold = 4,
        Cancelled = 5,
        Deferred = 6
    }
}