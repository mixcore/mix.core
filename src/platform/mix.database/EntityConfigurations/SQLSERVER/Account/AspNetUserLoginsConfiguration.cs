using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;
using Mix.Database.EntityConfigurations.SQLSERVER;

namespace Mix.Database.EntityConfigurations.Account.SQLSERVER
{
    internal class AspNetUserLoginsConfiguration : AspNetUserLoginsConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetUserLogins> builder)
        {
            base.Configure(builder);
        }
    }
}
