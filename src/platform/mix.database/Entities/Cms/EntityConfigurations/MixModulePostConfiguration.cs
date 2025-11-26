using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixModulePostConfiguration : AssociationBaseConfiguration<MixModulePostAssociation, int>

    {
        public MixModulePostConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixModulePostAssociation> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.MixModuleContentId)
              .HasColumnName("mix_module_content_id")
              .HasColumnType(Config.Integer);

        }
    }
}
