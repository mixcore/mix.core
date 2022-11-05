using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixLanguageContentConfiguration : MixLanguageContentConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixLanguageContent> builder)
        {
            base.Configure(builder);
        }
    }
}
