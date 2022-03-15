using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Account
{
    internal class AspNetUserLoginsConfiguration : AspNetUserLoginsConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetUserLogins> builder)
        {
            base.Configure(builder);
        }
    }
}
