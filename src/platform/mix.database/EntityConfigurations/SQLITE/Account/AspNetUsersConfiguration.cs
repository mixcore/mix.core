using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.SQLITE.Account
{
    internal class AspNetUsersConfiguration : MixUsersConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixUser> builder)
        {
            base.Configure(builder);
        }
    }
}
