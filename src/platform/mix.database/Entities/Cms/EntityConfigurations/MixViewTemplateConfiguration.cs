using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixViewTemplateContentConfiguration : EntityBaseConfiguration<MixTemplate, int>
    {
        public MixViewTemplateContentConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixTemplate> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.TenantId)
              .HasColumnName("tenant_id")
              .HasColumnType(Config.Integer);
             builder.Property(e => e.MixThemeId)
              .HasColumnName("mix_theme_id")
              .HasColumnType(Config.Integer);
            builder.Property(e => e.Content)
                .HasColumnName("content")
                .HasColumnType(Config.Text)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Extension)
                .IsRequired()
                .HasColumnName("extension")
                .HasColumnType($"{Config.NString}{Config.SmallLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.FileFolder)
               .IsRequired()
               .HasColumnName("file_folder")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.FileName)
               .IsRequired()
               .HasColumnName("file_name")
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.FolderType)
                .HasColumnName("folder_type")
              .HasConversion(new EnumToStringConverter<MixTemplateFolderType>())
              .HasColumnType($"{Config.NString}{Config.SmallLength}")
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Scripts)
                .IsRequired()
                .HasColumnName("scripts")
                .HasColumnType(Config.Text)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Styles)
                .IsRequired()
                .HasColumnName("styles")
                .HasColumnType(Config.Text)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.MixThemeName)
               .IsRequired()
               .HasColumnName("mix_theme_name")
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);
        }
    }
}
