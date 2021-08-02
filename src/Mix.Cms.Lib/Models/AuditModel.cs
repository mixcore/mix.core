using Microsoft.EntityFrameworkCore;
using System;

namespace Mix.Cms.Lib.Models
{
    public class AuditContext: DbContext
    {
        public DbSet<AuditLog> AuditLog { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string cnn = $"Data Source=MixContent/auditlog_{DateTime.Now.ToString("MM_yyyy")}.db";
            optionsBuilder.UseSqlite(cnn);
        }
    }

    public class AuditLog
    {
        public Guid Id { get; set; }
        public string RequestIp { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string Body { get; set; }
        public bool Success { get; set; }
        public string Exception { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime{ get; set; }
    }
}
