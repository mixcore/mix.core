using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.POSTGRESQL
{
    public class MixAttributeFieldConfiguration : IEntityTypeConfiguration<MixAttributeField>
    {
        public void Configure(EntityTypeBuilder<MixAttributeField> entity)
        {
            entity.ToTable("mix_attribute_field");

            entity.HasIndex(e => e.AttributeSetId);

            entity.HasIndex(e => e.ReferenceId);

            entity.Property(e => e.AttributeSetName)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.CreatedDateTime).HasColumnType("timestamp without time zone");

            entity.Property(e => e.DefaultValue)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.IsEncrypt).HasColumnType("boolean");

            entity.Property(e => e.IsMultiple).HasColumnType("boolean");

            entity.Property(e => e.IsRequire).HasColumnType("boolean");

            entity.Property(e => e.IsSelect).HasColumnType("boolean");

            entity.Property(e => e.IsUnique).HasColumnType("boolean");

            entity.Property(e => e.LastModified).HasColumnType("timestamp without time zone");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.Options)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.Regex)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.DataType)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixDataType>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.Title)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");
        }
    }
}