using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixViewTemplateContentConfiguration : MixViewTemplateContentConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixTemplate> builder)
        {
            base.Configure(builder);
        }
    }
}
