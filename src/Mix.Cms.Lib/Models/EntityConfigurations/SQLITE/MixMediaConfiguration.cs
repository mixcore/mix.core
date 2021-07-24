using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.SQLITE
{
    public class MixMediaConfiguration : IEntityTypeConfiguration<MixMedia>
    {
        public void Configure(EntityTypeBuilder<MixMedia> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture })
                   .HasName("PK_mix_media");

            entity.ToTable("mix_media");

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.Property(e => e.Specificulture)
                .HasColumnType("varchar(10)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

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

            entity.Property(e => e.FileProperties)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.FileType)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Source)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Tags)
                .HasColumnType("varchar(400)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.TargetUrl)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Title)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");
        }
    }
}