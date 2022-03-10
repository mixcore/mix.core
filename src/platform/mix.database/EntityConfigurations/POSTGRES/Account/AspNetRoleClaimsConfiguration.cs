using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;
using Mix.Database.EntityConfigurations.POSTGRES;

namespace Mix.Database.EntityConfigurations.Account.POSTGRES
{
    internal class AspNetRoleClaimsConfiguration : AspNetRoleClaimsConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetRoleClaims> builder)
        {
            base.Configure(builder);
        }
    }
}
