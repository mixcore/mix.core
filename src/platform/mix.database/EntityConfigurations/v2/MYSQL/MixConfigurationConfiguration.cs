using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixConfigurationConfiguration : SiteEntityBaseConfiguration<MixConfiguration, int>
    {
        public override void Configure(EntityTypeBuilder<MixConfiguration> builder)
        {
            base.Configure(builder);
        }
    }
}
