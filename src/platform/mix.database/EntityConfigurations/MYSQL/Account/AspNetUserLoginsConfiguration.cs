using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.MYSQL.Account
{
    internal class AspNetUserLoginsConfiguration : AspNetUserLoginsConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<AspNetUserLogins> builder)
        {
            base.Configure(builder);
        }
    }
}
