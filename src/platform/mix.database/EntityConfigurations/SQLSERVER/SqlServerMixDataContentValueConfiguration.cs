using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixDataContentValueConfiguration : MixDataContentValueConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDataContentValue> builder)
        {
            base.Configure(builder);
        }
    }
}
