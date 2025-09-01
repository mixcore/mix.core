using Mix.Database.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mix.Database.Entities.Compliance
{
    public class BreakGlassAccess : TenantEntityBase<int>
    {
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string AccessReason { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(500)]
        public string Justification { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string ApprovedBy { get; set; } = string.Empty;
        
        public DateTime AccessStartTime { get; set; }
        public DateTime AccessEndTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        
        [Required]
        public BreakGlassStatus Status { get; set; }
        
        [MaxLength(45)]
        public string? IpAddress { get; set; }
        
        [MaxLength(500)]
        public string? UserAgent { get; set; }
        
        public virtual ICollection<BreakGlassAudit>? AuditTrail { get; set; }
    }
    
    public enum BreakGlassStatus
    {
        Requested = 1,
        Approved = 2,
        Active = 3,
        Expired = 4,
        Revoked = 5
    }
}