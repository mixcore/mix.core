using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixContributorConfiguration : MixContributorConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixContributor> builder)
        {
            base.Configure(builder);
        }
    }
}
