using Mix.Database.Entities.AuditLog.EntityConfigurations;
using Mix.Database.Services;
using Mix.Heart.Services;

namespace Mix.Database.Entities.AuditLog
{
    public class AuditLogDbContext : BaseDbContext
    {
        public AuditLogDbContext(DatabaseService databaseService) : base(databaseService, MixConstants.CONST_AUDIT_LOG_CONNECTION)
        {
        }

        public AuditLogDbContext(DatabaseService databaseService, string connectionStringName) : base(databaseService, connectionStringName)
        {
        }

        public AuditLogDbContext(string connectionString, MixDatabaseProvider databaseProvider) : base(connectionString, databaseProvider)
        {
        }

        public virtual DbSet<AuditLog> AuditLog { get; set; }

    }
}
