using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixLanguageConfiguration : MixLanguageConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixLanguage> builder)
        {
            base.Configure(builder);
        }
    }
}
