using Mix.Database.EntityConfigurations.SQLITE.Base;

namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixDataConfiguration : MixDataConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixData> builder)
        {
            base.Configure(builder);
        }
    }
}
