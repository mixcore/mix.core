using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixDatabaseConfiguration : MixDatabaseConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabase> builder)
        {
            base.Configure(builder);
        }
    }
}
