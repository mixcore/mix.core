using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.SQLITE.Base;

namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class MixDomainConfiguration : SqliteTenantEntityBaseConfiguration<MixDomain, int>
    {
        public override void Configure(EntityTypeBuilder<MixDomain> builder)
        {
            base.Configure(builder);
        }
    }
}
