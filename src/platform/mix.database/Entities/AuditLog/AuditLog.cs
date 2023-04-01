namespace Mix.Database.Entities.AuditLog
{
    public class AuditLog: EntityBase<Guid>
    {
        public bool Success { get; set; }
        public string RequestIp { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string Body { get; set; }
        public string Exception { get; set; }
    }
}
