using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;
using Mix.Database.EntityConfigurations.MYSQL;

namespace Mix.Database.EntityConfigurations.Account.MYSQL
{
    internal class ClientsConfiguration : ClientsConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<Clients> builder)
        {
            base.Configure(builder);
        }
    }
}
