using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.SQLITE;
using Mix.Database.Services;
using Mix.Shared.Services;

namespace Mix.Database.Entities.v2
{
    public class SqliteMixCmsContext : MixCmsContext
    {
        public SqliteMixCmsContext(MixDatabaseService databaseService, GlobalConfigService globalConfigService) 
            : base(databaseService, globalConfigService)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                this.GetType().Assembly, 
                m=> m.Namespace == typeof(SqliteDatabaseConstants).Namespace);
        }
    }
}
