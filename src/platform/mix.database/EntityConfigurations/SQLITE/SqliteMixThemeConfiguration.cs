namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixThemeConfiguration : MixThemeConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixTheme> builder)
        {
            base.Configure(builder);
        }
    }
}
