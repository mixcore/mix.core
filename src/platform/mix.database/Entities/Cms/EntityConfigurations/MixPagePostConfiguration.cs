using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixPagePostConfiguration : AssociationBaseConfiguration<MixPagePostAssociation, int>

    {
        public MixPagePostConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixPagePostAssociation> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.MixPageContentId)
              .HasColumnName("mix_page_content_id")
              .HasColumnType(Config.Integer);
        }
    }
}
