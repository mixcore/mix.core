using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.POSTGRES.Account
{
    internal class RefreshTokensConfiguration : RefreshTokensConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<RefreshTokens> builder)
        {
            base.Configure(builder);
        }
    }
}
