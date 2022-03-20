using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.MYSQL.Account
{
    internal class AspNetUsersConfiguration : MixUsersConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixUser> builder)
        {
            base.Configure(builder);
        }
    }
}
