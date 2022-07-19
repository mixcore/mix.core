namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixLanguageConfiguration : MixLanguageConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixLanguage> builder)
        {
            base.Configure(builder);
        }
    }
}
