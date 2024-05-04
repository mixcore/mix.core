using Mix.Database.Services;

namespace Mix.Database.Entities.AuditLog
{
    public class PostgresAuditLogDbContext : AuditLogDbContext
    {
        public PostgresAuditLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
