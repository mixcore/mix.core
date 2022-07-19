namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixContributorConfiguration : MixContributorConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixContributor> builder)
        {
            base.Configure(builder);
        }
    }
}
