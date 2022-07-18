namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixTenantConfiguration : MixTenantConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixTenant> builder)
        {
            base.Configure(builder);
        }
    }
}
