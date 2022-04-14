using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.SQLSERVER.Base;


namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class MixViewTemplateContentConfiguration : SqlServerEntityBaseConfiguration<MixTemplate, int>
    {
        public override void Configure(EntityTypeBuilder<MixTemplate> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Content)
                .HasColumnType(Config.Text)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Extension)
                .IsRequired()
                .HasColumnType($"{Config.NString}{Config.SmallLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.FileFolder)
               .IsRequired()
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.FileName)
               .IsRequired()
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.FolderType)
              .HasConversion(new EnumToStringConverter<MixTemplateFolderType>())
              .HasColumnType($"{Config.NString}{Config.SmallLength}")
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Scripts)
                .IsRequired()
                .HasColumnType(Config.Text)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Styles)
                .IsRequired()
                .HasColumnType(Config.Text)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.MixThemeName)
               .IsRequired()
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);
        }
    }
}
