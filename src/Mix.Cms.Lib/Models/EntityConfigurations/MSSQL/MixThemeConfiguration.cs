using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.MSSQL
{
    public class MixThemeConfiguration : IEntityTypeConfiguration<MixTheme>
    {
        public void Configure(EntityTypeBuilder<MixTheme> entity)
        {
            entity.ToTable("mix_theme");

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Image)
                .HasColumnType("nvarchar(250)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("nvarchar(250)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.PreviewUrl)
                .HasColumnType("nvarchar(400)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Thumbnail)
                .HasColumnType("nvarchar(250)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Title)
                .HasColumnType("nvarchar(250)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");
        }
    }
}