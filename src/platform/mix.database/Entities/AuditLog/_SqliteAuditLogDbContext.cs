using Mix.Database.Entities.AuditLog.EntityConfigurations;
using Mix.Database.Services;
using Mix.Heart.Services;

namespace Mix.Database.Entities.AuditLog
{
    public class SqlITEAuditLogDbContext : AuditLogDbContext
    {
        public SqlITEAuditLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
