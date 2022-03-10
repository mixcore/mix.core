using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;
using Mix.Database.EntityConfigurations.SQLSERVER;

namespace Mix.Database.EntityConfigurations.Account.SQLSERVER
{
    internal class AspNetRolesConfiguration : AspNetRolesConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetRoles> builder)
        {
            base.Configure(builder);
        }
    }
}
