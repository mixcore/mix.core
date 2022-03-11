using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;
using Mix.Database.EntityConfigurations.MYSQL;

namespace Mix.Database.EntityConfigurations.MYSQL.Account
{
    internal class AspNetUserRolesConfiguration : AspNetUserRolesConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetUserRoles> builder)
        {
            base.Configure(builder);
        }
    }
}
