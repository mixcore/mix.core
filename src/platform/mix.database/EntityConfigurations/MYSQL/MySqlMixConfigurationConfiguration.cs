namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixConfigurationConfiguration : MixConfigurationConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixConfiguration> builder)
        {
            base.Configure(builder);
        }
    }
}
