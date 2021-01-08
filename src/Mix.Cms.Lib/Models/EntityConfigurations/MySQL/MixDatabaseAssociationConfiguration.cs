using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Cms;

using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.EntityConfigurations.MySQL
{
    public class MixDatabaseAssociationConfiguration : IEntityTypeConfiguration<MixDatabaseAssociation>
    {
        public void Configure(EntityTypeBuilder<MixDatabaseAssociation> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture })
                    .HasName("PRIMARY");

            entity.ToTable("mix_related_attribute_set");

            entity.Property(e => e.Specificulture)
                .HasColumnType("varchar(10)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Description)
                .HasColumnType("varchar(450)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.Image)
                .HasColumnType("varchar(450)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");
        }
    }
}
