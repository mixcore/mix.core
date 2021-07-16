using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES;
using Mix.Database.Services;
using Mix.Shared.Services;

namespace Mix.Database.Entities.v2
{
    public class PostgresqlMixCmsContext : MixCmsContext
    {
        public PostgresqlMixCmsContext(MixDatabaseService databaseService, MixAppSettingService appSettingService) 
            : base(databaseService, appSettingService){
            
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
