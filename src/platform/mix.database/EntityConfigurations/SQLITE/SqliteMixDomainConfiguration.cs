namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixDomainConfiguration : MixDomainConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDomain> builder)
        {
            base.Configure(builder);
        }
    }
}
