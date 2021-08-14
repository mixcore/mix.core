using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.SQLSERVER;
using Mix.Database.Services;
using Mix.Shared.Services;

namespace Mix.Database.Entities.v2
{
    public class SqlServerMixCmsContext : MixCmsContext
    {
        public SqlServerMixCmsContext(MixDatabaseService databaseService, GlobalConfigService globalConfigService) 
            : base(databaseService, globalConfigService)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                this.GetType().Assembly,
                m => m.Namespace == typeof(SqlServerDatabaseConstants).Namespace);
        }
    }
}
