using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.MYSQL.Account
{
    internal class ClientsConfiguration : ClientsConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<Clients> builder)
        {
            base.Configure(builder);
        }
    }
}
