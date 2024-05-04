using Mix.Database.Services;

namespace Mix.Database.Entities.AuditLog
{
    public class SqlITEAuditLogDbContext : AuditLogDbContext
    {
        public SqlITEAuditLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
