using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixConfigurationContentConfiguration : MixConfigurationContentConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixConfigurationContent> builder)
        {
            base.Configure(builder);
        }
    }
}
