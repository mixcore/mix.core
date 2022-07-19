namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixCultureConfiguration : MixCultureConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixCulture> builder)
        {
            base.Configure(builder);
        }
    }
}
