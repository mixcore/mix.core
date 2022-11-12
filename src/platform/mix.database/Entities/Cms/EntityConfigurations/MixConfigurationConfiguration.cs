using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixConfigurationConfiguration : TenantEntityUniqueNameBaseConfiguration<MixConfiguration, int>

    {
        public MixConfigurationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixConfiguration> builder)
        {
            base.Configure(builder);
        }
    }
}
