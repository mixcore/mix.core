using Mix.Database.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mix.Database.Entities.Compliance
{
    public class Purpose : TenantEntityBase<int>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public override string Description { get; set; } = string.Empty;
        
        [Required]
        public LawfulBasisType LawfulBasis { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public virtual ICollection<DataFieldMetadata>? DataFields { get; set; }
        public virtual ICollection<ConsentEvent>? ConsentEvents { get; set; }
    }
    
    public enum LawfulBasisType
    {
        Consent = 1,
        Contract = 2,
        LegalObligation = 3,
        VitalInterests = 4,
        PublicTask = 5,
        LegitimateInterests = 6
    }
}