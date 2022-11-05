using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixDataContentValueConfiguration : MixDataContentValueConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDataContentValue> builder)
        {
            base.Configure(builder);
        }
    }
}
