using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixPageConfiguration : MixPageConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixPage> builder)
        {
            base.Configure(builder);
        }
    }
}
