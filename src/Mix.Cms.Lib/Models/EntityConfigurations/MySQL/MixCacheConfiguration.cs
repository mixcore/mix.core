using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.MySQL
{
    public class MixCacheConfiguration : IEntityTypeConfiguration<MixCache>
    {
        public void Configure(EntityTypeBuilder<MixCache> entity)
        {
            entity.ToTable("mix_cache");

            entity.HasIndex(e => e.ExpiredDateTime)
                .HasName("Index_ExpiresAtTime");

            entity.Property(e => e.Id)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.ExpiredDateTime).HasColumnType("datetime");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixEnums.MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.Value)
                .IsRequired()
                .HasColumnType("text")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");
        }
    }
}
