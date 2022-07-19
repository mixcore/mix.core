namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixConfigurationConfiguration : MixConfigurationConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixConfiguration> builder)
        {
            base.Configure(builder);
        }
    }
}
