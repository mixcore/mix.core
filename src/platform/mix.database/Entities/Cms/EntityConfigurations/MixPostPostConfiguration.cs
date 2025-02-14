using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixPostPostConfiguration : AssociationBaseConfiguration<MixPostPostAssociation, int>

    {
        public MixPostPostConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixPostPostAssociation> builder)
        {
            base.Configure(builder);
        }
    }
}
