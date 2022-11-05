using Mix.Database.Base;
using Mix.Database.EntityConfigurations.POSTGRES.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixDataConfiguration : MixDataConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixData> builder)
        {
            base.Configure(builder);
        }
    }
}
