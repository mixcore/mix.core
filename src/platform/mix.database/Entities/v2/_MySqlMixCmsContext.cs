using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL;
using Mix.Database.Services;
using Mix.Shared.Services;

namespace Mix.Database.Entities.v2
{
    public class MySqlMixCmsContext : MixCmsContext
    {
        public MySqlMixCmsContext(MixDatabaseService databaseService, MixAppSettingService appSettingService) 
            : base(databaseService, appSettingService)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                this.GetType().Assembly,
                m => m.Namespace == typeof(MySqlDatabaseConstants).Namespace);
        }
    }
}
