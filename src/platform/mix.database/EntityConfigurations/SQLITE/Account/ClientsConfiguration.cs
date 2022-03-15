using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.SQLITE.Account
{
    internal class ClientsConfiguration : ClientsConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<Clients> builder)
        {
            base.Configure(builder);
        }
    }
}
