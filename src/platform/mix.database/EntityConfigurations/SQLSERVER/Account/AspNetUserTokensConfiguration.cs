using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;
using Mix.Database.EntityConfigurations.SQLSERVER;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Account
{
    internal class AspNetUserTokensConfiguration : AspNetUserTokensConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetUserTokens> builder)
        {
            base.Configure(builder);
        }
    }
}
