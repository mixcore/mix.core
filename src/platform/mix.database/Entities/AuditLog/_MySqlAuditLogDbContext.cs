using Mix.Database.Entities.AuditLog.EntityConfigurations;
using Mix.Database.Services;
using Mix.Heart.Services;

namespace Mix.Database.Entities.AuditLog
{
    public class MySqlAuditLogDbContext : AuditLogDbContext
    {
        public MySqlAuditLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
