using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.POSTGRES.Account
{
    internal class MixUserTenantConfiguration : MixUserTenantConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixUserTenant> builder)
        {
            base.Configure(builder);
        }
    }
}
