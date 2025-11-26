using Mix.Database.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mix.Database.Entities.Compliance
{
    public class DsrRequest : TenantEntityBase<int>
    {
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public DsrRequestType RequestType { get; set; }
        
        [Required]
        public DsrRequestStatus Status { get; set; } = DsrRequestStatus.Pending;
        
        public DateTime SubmittedUtc { get; set; }
        public DateTime DueUtc { get; set; }
        public DateTime? ProcessedUtc { get; set; }
        
        public bool SlaMetricMet { get; set; }
        
        [MaxLength(1000)]
        public string? Notes { get; set; }
        
        [MaxLength(500)]
        public string? ProcessedBy { get; set; }
        
        public string? ExportFilePath { get; set; } // For access requests
    }
    
    public enum DsrRequestType
    {
        Access = 1,
        Erasure = 2,
        Rectification = 3,
        Restriction = 4,
        Portability = 5,
        Objection = 6
    }
    
    public enum DsrRequestStatus
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3,
        Rejected = 4
    }
}