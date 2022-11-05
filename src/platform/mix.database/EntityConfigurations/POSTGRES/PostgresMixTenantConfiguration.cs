using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixTenantConfiguration : MixTenantConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixTenant> builder)
        {
            base.Configure(builder);
        }
    }
}
