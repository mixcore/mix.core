using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.SQLITE
{
    public class MixTemplateConfiguration : IEntityTypeConfiguration<MixTemplate>
    {
        public void Configure(EntityTypeBuilder<MixTemplate> entity)
        {
            entity.ToTable("mix_template");

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.HasIndex(e => e.ThemeId)
                .HasDatabaseName("IX_mix_template_file_TemplateId");

            entity.Property(e => e.Content)
                .IsRequired()
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Extension)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.FileFolder)
                .IsRequired()
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.FileName)
                .IsRequired()
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.FolderType)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.MobileContent)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Scripts)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.SpaContent)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Styles)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.ThemeName)
                .IsRequired()
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.HasOne(d => d.Theme)
                .WithMany(p => p.MixTemplate)
                .HasForeignKey(d => d.ThemeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mix_template_mix_theme");
        }
    }
}