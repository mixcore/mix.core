using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.SQLSERVER.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class MixViewTemplateContentConfiguration : SqlServerEntityBaseConfiguration<MixViewTemplate, int>
    {
        public override void Configure(EntityTypeBuilder<MixViewTemplate> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Content)
                .HasColumnType(_config.Text)
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Extension)
                .IsRequired()
                .HasColumnType($"{_config.NString}{_config.SmallLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

             builder.Property(e => e.FileFolder)
                .IsRequired()
                .HasColumnType($"{_config.NString}{_config.MediumLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

             builder.Property(e => e.FileName)
                .IsRequired()
                .HasColumnType($"{_config.NString}{_config.SmallLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.FolderType)
              .HasConversion(new EnumToStringConverter<MixTemplateFolderType>())
              .HasColumnType($"{_config.NString}{_config.SmallLength}")
              .HasCharSet(_config.CharSet)
              .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Scripts)
                .IsRequired()
                .HasColumnType(_config.Text)
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Styles)
                .IsRequired()
                .HasColumnType(_config.Text)
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);
            
            builder.Property(e => e.SpaContent)
                .IsRequired()
                .HasColumnType(_config.Text)
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.MobileContent)
                .IsRequired()
                .HasColumnType(_config.Text)
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

             builder.Property(e => e.MixThemeName)
                .IsRequired()
                .HasColumnType($"{_config.NString}{_config.SmallLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);
        }
    }
}
