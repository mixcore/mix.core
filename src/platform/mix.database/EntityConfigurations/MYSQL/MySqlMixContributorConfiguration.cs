namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixContributorConfiguration : MixContributorConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixContributor> builder)
        {
            base.Configure(builder);
        }
    }
}
