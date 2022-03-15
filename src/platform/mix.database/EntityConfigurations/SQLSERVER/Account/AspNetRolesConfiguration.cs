using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Account
{
    internal class AspNetRolesConfiguration : AspNetRolesConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetRoles> builder)
        {
            base.Configure(builder);
        }
    }
}
