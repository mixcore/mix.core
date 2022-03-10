using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;
using Mix.Database.EntityConfigurations.POSTGRES;

namespace Mix.Database.EntityConfigurations.Account.POSTGRES
{
    internal class AspNetRolesConfiguration : AspNetRolesConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetRoles> builder)
        {
            base.Configure(builder);
        }
    }
}
