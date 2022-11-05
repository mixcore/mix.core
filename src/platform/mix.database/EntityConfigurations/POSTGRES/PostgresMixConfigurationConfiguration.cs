using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixConfigurationConfiguration : MixConfigurationConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixConfiguration> builder)
        {
            base.Configure(builder);
        }
    }
}
