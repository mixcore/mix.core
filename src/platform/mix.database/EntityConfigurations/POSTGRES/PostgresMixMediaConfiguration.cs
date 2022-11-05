using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixMediaConfiguration : MixMediaConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixMedia> builder)
        {
            base.Configure(builder);
        }
    }
}
