namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixTenantConfiguration : MixTenantConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixTenant> builder)
        {
            base.Configure(builder);
        }
    }
}
