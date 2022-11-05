using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixDataContentConfiguration : MixDataContentConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDataContent> builder)
        {
            base.Configure(builder);
        }
    }
}
