using Newtonsoft.Json.Linq;

namespace Mix.Database.Entities.AuditLog
{
    public class AuditLog : EntityBase<Guid>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string RequestIp { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string QueryString { get; set; }
        public JObject Body { get; set; }
        public JObject Response { get; set; }
        public JObject Exception { get; set; }
    }
}
