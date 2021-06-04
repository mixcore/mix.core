using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.SQLSERVER.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixConfigurationConfiguration : EntityBaseConfiguration<MixConfiguration, int>
    {
        public override void Configure(EntityTypeBuilder<MixConfiguration> builder)
        {
        }
    }
}
