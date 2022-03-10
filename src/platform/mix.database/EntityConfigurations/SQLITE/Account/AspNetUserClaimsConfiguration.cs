using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;
using Mix.Database.EntityConfigurations.SQLITE;

namespace Mix.Database.EntityConfigurations.Account.SQLITE
{
    internal class AspNetUserClaimsConfiguration : AspNetUserClaimsConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetUserClaims> builder)
        {
            base.Configure(builder);
        }
    }
}
