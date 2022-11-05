using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixUrlAliasConfiguration : MixUrlAliasConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixUrlAlias> builder)
        {
            base.Configure(builder);
        }
    }
}
