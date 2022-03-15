using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.MYSQL.Account
{
    internal class AspNetUserClaimsConfiguration : AspNetUserClaimsConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetUserClaims> builder)
        {
            base.Configure(builder);
        }
    }
}
