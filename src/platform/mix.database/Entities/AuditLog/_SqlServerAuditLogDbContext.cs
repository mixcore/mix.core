using Mix.Database.Services;

namespace Mix.Database.Entities.AuditLog
{
    public class SqlServerAuditLogDbContext : AuditLogDbContext
    {
        public SqlServerAuditLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
