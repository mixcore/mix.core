namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixThemeConfiguration : MixThemeConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixTheme> builder)
        {
            base.Configure(builder);
        }
    }
}
