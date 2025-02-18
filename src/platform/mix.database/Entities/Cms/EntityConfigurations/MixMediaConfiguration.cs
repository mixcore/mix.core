using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixMediaConfiguration : TenantEntityBaseConfiguration<MixMedia, Guid>

    {
        public MixMediaConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixMedia> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Extension)
                .HasColumnName("extension")
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.FileFolder)
                .HasColumnName("file_folder")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet);

            builder.Property(e => e.FileName)
                .HasColumnName("file_name")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.FileProperties)
                .HasColumnName("file_properties")
               .HasColumnType($"{Config.NString}{Config.MaxLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.FileSize)
                .HasColumnName("file_size");

            builder.Property(e => e.FileType)
            .HasColumnName("file_type")
           .HasColumnType($"{Config.NString}{Config.SmallLength}")
           .HasCharSet(Config.CharSet)
           .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.DisplayName)
                .HasColumnName("display_name")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Source)
                .HasColumnName("source")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.TargetUrl)
                .HasColumnName("target_url")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Tags)
                .HasColumnName("tags")
               .HasColumnType($"{Config.NString}{Config.MaxLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);
        }
    }
}
