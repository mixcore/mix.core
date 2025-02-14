using Mix.Database.Entities.AuditLog.EntityConfigurations;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Services;

namespace Mix.Database.Entities.AuditLog
{
    public class SqliteAuditLogDbContext : AuditLogDbContext
    {
        public SqliteAuditLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
