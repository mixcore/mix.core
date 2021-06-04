using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixUrlAliasConfiguration : SiteEntityBaseConfiguration<MixUrlAlias, int>
    {
        public override void Configure(EntityTypeBuilder<MixUrlAlias> builder)
        {
            base.Configure(builder);
        }
    }
}
