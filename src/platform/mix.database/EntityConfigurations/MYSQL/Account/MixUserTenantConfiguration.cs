using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.MYSQL.Account
{
    internal class MixUserTenantConfiguration : MixUserTenantConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixUserTenant> builder)
        {
            base.Configure(builder);
        }
    }
}
