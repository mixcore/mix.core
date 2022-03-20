using Mix.Database.EntityConfigurations.SQLSERVER;
using Mix.Database.Services;

namespace Mix.Database.Entities
{
    public class SqlServerMixCmsContext : MixCmsContext
    {
        public SqlServerMixCmsContext(MixDatabaseService databaseService)
            : base(databaseService)
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
