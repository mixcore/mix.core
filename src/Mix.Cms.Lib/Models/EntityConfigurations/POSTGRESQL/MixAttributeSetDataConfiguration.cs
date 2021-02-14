using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.POSTGRESQL
{
    public class MixAttributeSetDataConfiguration : IEntityTypeConfiguration<MixAttributeSetData>
    {
        public void Configure(EntityTypeBuilder<MixAttributeSetData> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture })
                    .HasName("PK_mix_attribute_set_data");

            entity.ToTable("mix_attribute_set_data");

            entity.HasIndex(e => e.AttributeSetId);

            entity.Property(e => e.Id)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.Specificulture)
                .HasColumnType("varchar(10)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.AttributeSetName)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.CreatedDateTime).HasColumnType("timestamp without time zone");

            entity.Property(e => e.LastModified).HasColumnType("timestamp without time zone");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("und-x-icu");
        }
    }
}