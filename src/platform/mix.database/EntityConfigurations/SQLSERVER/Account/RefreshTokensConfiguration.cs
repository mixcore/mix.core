using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Account
{
    internal class RefreshTokensConfiguration : RefreshTokensConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<RefreshTokens> builder)
        {
            base.Configure(builder);
        }
    }
}
