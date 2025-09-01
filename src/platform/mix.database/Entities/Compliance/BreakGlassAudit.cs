using Mix.Database.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mix.Database.Entities.Compliance
{
    public class BreakGlassAudit : TenantEntityBase<int>
    {
        public int BreakGlassAccessId { get; set; }
        public virtual BreakGlassAccess? BreakGlassAccess { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Action { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? EntityType { get; set; }
        
        [MaxLength(100)]
        public string? EntityId { get; set; }
        
        [MaxLength(1000)]
        public string? Details { get; set; }
        
        public DateTime ActionTimestamp { get; set; }
        
        [MaxLength(45)]
        public string? IpAddress { get; set; }
        
        public bool PhiAccessed { get; set; }
    }
}