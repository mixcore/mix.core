using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.SQLITE;
using Mix.Database.Services;

namespace Mix.Database.Entities.v2
{
    public class SqliteMixCmsContext : MixCmsContext
    {
        public SqliteMixCmsContext(MixDatabaseService databaseService)
            : base(databaseService)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                this.GetType().Assembly,
                m => m.Namespace == typeof(SqliteDatabaseConstants).Namespace);
        }
    }
}
