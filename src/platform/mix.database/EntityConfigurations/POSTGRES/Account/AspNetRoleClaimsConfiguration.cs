using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.POSTGRES.Account
{
    internal class AspNetRoleClaimsConfiguration : AspNetRoleClaimsConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetRoleClaims> builder)
        {
            base.Configure(builder);
        }
    }
}
