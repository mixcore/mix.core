using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixThemeConfiguration : MixThemeConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixTheme> builder)
        {
            base.Configure(builder);
        }
    }
}
