using Mix.Database.Entities.AuditLog.EntityConfigurations;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Services;

namespace Mix.Database.Entities.AuditLog
{
    public class SqlServerAuditLogDbContext : AuditLogDbContext
    {
        public SqlServerAuditLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
