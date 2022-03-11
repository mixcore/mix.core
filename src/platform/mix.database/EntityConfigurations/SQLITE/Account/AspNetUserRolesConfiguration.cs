using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;
using Mix.Database.EntityConfigurations.SQLITE;

namespace Mix.Database.EntityConfigurations.SQLITE.Account
{
    internal class AspNetUserRolesConfiguration : AspNetUserRolesConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetUserRoles> builder)
        {
            base.Configure(builder);
        }
    }
}
