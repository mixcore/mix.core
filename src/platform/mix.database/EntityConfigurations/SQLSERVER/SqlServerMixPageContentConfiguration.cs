using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixPageContentConfiguration : MixPageContentConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixPageContent> builder)
        {
            base.Configure(builder);
        }
    }
}
