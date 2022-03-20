using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.SQLITE.Account
{
    internal class MixUserTenantConfiguration : MixUserTenantConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixUserTenant> builder)
        {
            base.Configure(builder);
        }
    }
}
