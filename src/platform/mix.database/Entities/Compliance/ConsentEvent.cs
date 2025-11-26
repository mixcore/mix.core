using Mix.Database.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mix.Database.Entities.Compliance
{
    public class ConsentEvent : TenantEntityBase<int>
    {
        [Required]
        public Guid UserId { get; set; }
        
        public int PurposeId { get; set; }
        public virtual Purpose? Purpose { get; set; }
        
        public bool Granted { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Method { get; set; } = string.Empty; // "web_form", "api", "import", etc.
        
        [MaxLength(20)]
        public string? Version { get; set; }
        
        [MaxLength(45)]
        public string? IpAddress { get; set; }
        
        [MaxLength(500)]
        public string? UserAgent { get; set; }
        
        public DateTime ConsentTimestamp { get; set; }
    }
}