namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixConfigurationConfiguration : MixConfigurationConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixConfiguration> builder)
        {
            base.Configure(builder);
        }
    }
}
