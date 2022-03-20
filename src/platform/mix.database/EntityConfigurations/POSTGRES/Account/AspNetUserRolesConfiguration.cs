using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.POSTGRES.Account
{
    internal class AspNetUserRolesConfiguration : AspNetUserRolesConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetUserRoles> builder)
        {
            base.Configure(builder);
        }
    }
}
