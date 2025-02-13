using Mix.Database.Entities.AuditLog.EntityConfigurations;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Services;

namespace Mix.Database.Entities.AuditLog
{
    public class PostgresAuditLogDbContext : AuditLogDbContext
    {
        public PostgresAuditLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
