using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

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
