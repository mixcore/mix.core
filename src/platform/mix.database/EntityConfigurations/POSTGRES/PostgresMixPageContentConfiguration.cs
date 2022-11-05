using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixPageContentConfiguration : MixPageContentConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixPageContent> builder)
        {
            base.Configure(builder);
        }
    }
}
