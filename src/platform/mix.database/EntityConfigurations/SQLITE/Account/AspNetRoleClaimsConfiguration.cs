using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;
using Mix.Database.EntityConfigurations.SQLITE;

namespace Mix.Database.EntityConfigurations.SQLITE.Account
{
    internal class AspNetRoleClaimsConfiguration : AspNetRoleClaimsConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetRoleClaims> builder)
        {
            base.Configure(builder);
        }
    }
}
