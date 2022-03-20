using Mix.Database.EntityConfigurations.POSTGRES;
using Mix.Database.Services;

namespace Mix.Database.Entities
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
            modelBuilder.UseSerialColumns();

            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.ApplyConfigurationsFromAssembly(
                this.GetType().Assembly,
                m => m.Namespace == typeof(PostgresDatabaseConstants).Namespace);
        }
    }
}
