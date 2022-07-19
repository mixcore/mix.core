namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixContributorConfiguration : MixContributorConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixContributor> builder)
        {
            base.Configure(builder);
        }
    }
}
