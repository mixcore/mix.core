using Mix.Database.Base.Cms;
using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixLanguageConfiguration : TenantEntityUniqueNameBaseConfiguration<MixLanguage, int>

    {
        public MixLanguageConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixLanguage> builder)
        {
            base.Configure(builder);
        }
    }
}
