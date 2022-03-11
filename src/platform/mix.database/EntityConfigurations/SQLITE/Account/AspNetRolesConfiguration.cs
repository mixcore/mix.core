using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.SQLITE.Account
{
    internal class AspNetRolesConfiguration : AspNetRolesConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetRoles> builder)
        {
            base.Configure(builder);
        }
    }
}
