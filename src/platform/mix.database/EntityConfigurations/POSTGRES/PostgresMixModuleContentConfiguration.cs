using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixModuleContentConfiguration : MixModuleContentConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixModuleContent> builder)
        {
            base.Configure(builder);
        }
    }
}
