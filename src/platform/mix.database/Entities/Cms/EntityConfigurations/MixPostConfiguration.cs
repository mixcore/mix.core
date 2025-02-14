using Mix.Database.Base.Cms;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixPostConfiguration : TenantEntityBaseConfiguration<MixPost, int>

    {
        public MixPostConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixPost> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.DisplayName)
                .HasColumnName("display_name")
               .HasColumnType($"{Config.String}{Config.MaxLength}");
            builder.Property(e => e.Description)
                .HasColumnName("description")
               .HasColumnType($"{Config.String}{Config.MaxLength}");

        }
    }
}
