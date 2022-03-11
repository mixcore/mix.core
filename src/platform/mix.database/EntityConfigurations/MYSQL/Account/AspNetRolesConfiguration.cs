using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;
using Mix.Database.EntityConfigurations.MYSQL;

namespace Mix.Database.EntityConfigurations.MYSQL.Account
{
    internal class AspNetRolesConfiguration : AspNetRolesConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetRoles> builder)
        {
            base.Configure(builder);
        }
    }
}
