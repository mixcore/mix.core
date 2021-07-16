using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLITE.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLITE
{
    public class MixConfigurationConfiguration : SqliteSiteEntityUniqueNameBaseConfiguration<MixConfiguration, int>
    {
        public override void Configure(EntityTypeBuilder<MixConfiguration> builder)
        {
            base.Configure(builder);
        }
    }
}
