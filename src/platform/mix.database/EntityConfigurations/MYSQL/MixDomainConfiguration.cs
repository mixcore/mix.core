using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.MYSQL.Base;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MixDomainConfiguration : MySqlTenantEntityBaseConfiguration<MixDomain, int>
    {
        public override void Configure(EntityTypeBuilder<MixDomain> builder)
        {
            base.Configure(builder);
        }
    }
}
