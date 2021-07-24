using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.MSSQL
{
    public class MixFileConfiguration : IEntityTypeConfiguration<MixFile>
    {
        public void Configure(EntityTypeBuilder<MixFile> entity)
        {
            entity.ToTable("mix_file");

            entity.HasIndex(e => e.ThemeId);

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Extension)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.FileFolder)
                .IsRequired()
                .HasColumnType("nvarchar(250)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.FileName)
                .IsRequired()
                .HasColumnType("nvarchar(250)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.FolderType)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.StringContent)
                .IsRequired()
                .HasColumnType("ntext")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.ThemeName)
                .IsRequired()
                .HasColumnType("nvarchar(250)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.HasOne(d => d.Theme)
                .WithMany(p => p.MixFile)
                .HasForeignKey(d => d.ThemeId)
                .HasConstraintName("FK_mix_file_mix_template");
        }
    }
}