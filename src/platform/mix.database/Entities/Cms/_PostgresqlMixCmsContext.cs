using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.POSTGRES;
using Mix.Database.Services;

namespace Mix.Database.Entities.v2
{
    public class PostgresqlMixCmsContext : MixCmsContext
    {
        public PostgresqlMixCmsContext(MixDatabaseService databaseService)
            : base(databaseService)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.ApplyConfigurationsFromAssembly(
                this.GetType().Assembly,
                m => m.Namespace == typeof(PostgresDatabaseConstants).Namespace);
        }
    }
}
