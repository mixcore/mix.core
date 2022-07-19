namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixThemeConfiguration : MixThemeConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixTheme> builder)
        {
            base.Configure(builder);
        }
    }
}
