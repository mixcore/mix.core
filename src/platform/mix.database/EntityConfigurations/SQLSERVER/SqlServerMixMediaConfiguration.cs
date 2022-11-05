using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixMediaConfiguration : MixMediaConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixMedia> builder)
        {
            base.Configure(builder);
        }
    }
}
