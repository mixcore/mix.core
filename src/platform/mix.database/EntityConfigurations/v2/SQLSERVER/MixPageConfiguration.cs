using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLSERVER.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixPageConfiguration : SiteEntityBaseConfiguration<MixPage, int>
    {
        public override void Configure(EntityTypeBuilder<MixPage> builder)
        {
            base.Configure(builder);
        }
    }
}
