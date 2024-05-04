using Mix.Database.Services;

namespace Mix.Database.Entities.AuditLog
{
    public class MySqlAuditLogDbContext : AuditLogDbContext
    {
        public MySqlAuditLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
