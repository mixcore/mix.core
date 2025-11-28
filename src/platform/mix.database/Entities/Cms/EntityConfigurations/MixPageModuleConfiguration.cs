using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixPageModuleConfiguration : AssociationBaseConfiguration<MixPageModuleAssociation, int>

    {
        public MixPageModuleConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixPageModuleAssociation> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.MixPageContentId)
              .HasColumnName("mix_page_content_id")
              .HasColumnType(Config.Integer);
        }
    }
}
