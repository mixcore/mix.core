using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.POSTGRES.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class MixPageConfiguration : PostgresTenantEntityBaseConfiguration<MixPage, int>
    {
        public override void Configure(EntityTypeBuilder<MixPage> builder)
        {
            base.Configure(builder);
        }
    }
}
