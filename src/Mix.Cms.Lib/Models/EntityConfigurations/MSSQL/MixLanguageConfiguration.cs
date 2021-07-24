using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.MSSQL
{
    public class MixLanguageConfiguration : IEntityTypeConfiguration<MixLanguage>
    {
        public void Configure(EntityTypeBuilder<MixLanguage> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture })
                     .HasName("PK_mix_language");

            entity.ToTable("mix_language");

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.HasIndex(e => e.Specificulture);

            entity.Property(e => e.Specificulture)
                .HasColumnType("varchar(10)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Category)
                .HasColumnType("nvarchar(250)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.DefaultValue)
                .HasColumnType("nvarchar(250)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Description)
                .HasColumnType("nvarchar(250)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Keyword)
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

            entity.Property(e => e.DataType)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixDataType>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Value)
                .HasColumnType("ntext")
                .HasCharSet("utf8")
                .UseCollation("Vietnamese_CI_AS");

            entity.HasOne(d => d.SpecificultureNavigation)
                .WithMany(p => p.MixLanguage)
                .HasPrincipalKey(p => p.Specificulture)
                .HasForeignKey(d => d.Specificulture)
                .HasConstraintName("FK_Mix_Language_Mix_Culture");
        }
    }
}