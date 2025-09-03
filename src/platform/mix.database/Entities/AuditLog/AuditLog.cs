using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Mix.Database.Entities.AuditLog
{
    public class AuditLog: EntityBase<Guid>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public int ResponseTime { get; set; }
        public string RequestIp { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string QueryString { get; set; }
        public JObject Body { get; set; }
        public JObject Response { get; set; }
        public JObject Exception { get; set; }
        
        // HIPAA/GDPR Compliance Enhancement Fields
        [MaxLength(100)]
        public string CorrelationId { get; set; }
        
        public int? TenantId { get; set; }
        
        public bool PhiAccessFlag { get; set; }
        
        [MaxLength(500)]
        public string UserAgent { get; set; }
        
        [MaxLength(100)]
        public string SessionId { get; set; }
    }
}
