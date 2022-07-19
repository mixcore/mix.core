namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixLanguageConfiguration : MixLanguageConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixLanguage> builder)
        {
            base.Configure(builder);
        }
    }
}
