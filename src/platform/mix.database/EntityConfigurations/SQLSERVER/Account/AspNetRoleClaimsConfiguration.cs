using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;
using Mix.Database.EntityConfigurations.SQLSERVER;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Account
{
    internal class AspNetRoleClaimsConfiguration : AspNetRoleClaimsConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetRoleClaims> builder)
        {
            base.Configure(builder);
        }
    }
}
