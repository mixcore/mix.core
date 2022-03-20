using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.POSTGRES.Account
{
    internal class AspNetUsersConfiguration : MixUsersConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixUser> builder)
        {
            base.Configure(builder);
        }
    }
}
