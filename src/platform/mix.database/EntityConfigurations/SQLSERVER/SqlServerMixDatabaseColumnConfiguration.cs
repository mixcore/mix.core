using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixDatabaseColumnConfiguration : MixDatabaseColumnConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseColumn> builder)
        {
            base.Configure(builder);
        }
    }
}
