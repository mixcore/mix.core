using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixUrlAliasConfiguration : MixUrlAliasConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixUrlAlias> builder)
        {
            base.Configure(builder);
        }
    }
}
