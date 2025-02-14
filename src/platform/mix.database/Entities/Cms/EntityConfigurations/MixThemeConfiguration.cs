using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Newtonsoft.Json.Linq;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixThemeConfiguration : TenantEntityBaseConfiguration<MixTheme, int>
    {
        public MixThemeConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixTheme> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.MixDbId)
                .HasColumnName("mix_db_id");
            builder.Property(e => e.MixDatabaseName)
                .HasColumnName("mix_database_name")
               .HasColumnType($"{Config.NString}{Config.SmallLength}");
             builder.Property(e => e.SystemName)
                .HasColumnName("system_name")
               .HasColumnType($"{Config.NString}{Config.SmallLength}");

            builder.Property(e => e.PreviewUrl)
                .HasColumnName("preview_url")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.ImageUrl)
                .HasColumnName("image_url")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.AssetFolder)
                .HasColumnName("asset_folder")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.TemplateFolder)
                .HasColumnName("template_folder")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);


           
        }
    }
}
