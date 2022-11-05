using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixTenantConfiguration : MixTenantConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixTenant> builder)
        {
            base.Configure(builder);
        }
    }
}
