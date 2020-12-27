using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.MySQL
{
    public class MixAttributeSetConfiguration : IEntityTypeConfiguration<MixAttributeSet>
    {
        public void Configure(EntityTypeBuilder<MixAttributeSet> entity)
        {
            entity.ToTable("mix_attribute_set");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Description)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.EdmAutoSend).HasColumnType("bit(1)");

            entity.Property(e => e.EdmFrom)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.EdmSubject)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.EdmTemplate)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.FormTemplate)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.Type)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

        }
    }
}
