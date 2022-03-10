using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;
using Mix.Database.EntityConfigurations.SQLITE;

namespace Mix.Database.EntityConfigurations.Account.SQLITE
{
    internal class AspNetUserTokensConfiguration : AspNetUserTokensConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetUserTokens> builder)
        {
            base.Configure(builder);
        }
    }
}
