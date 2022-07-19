namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixDomainConfiguration : MixDomainConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDomain> builder)
        {
            base.Configure(builder);
        }
    }
}
