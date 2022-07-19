namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixLanguageConfiguration : MixLanguageConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixLanguage> builder)
        {
            base.Configure(builder);
        }
    }
}
