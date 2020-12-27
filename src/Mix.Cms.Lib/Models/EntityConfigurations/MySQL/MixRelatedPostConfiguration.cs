using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.MySQL
{
    public class MixRelatedPostConfiguration : IEntityTypeConfiguration<MixPortalPageRole>
    {
        public void Configure(EntityTypeBuilder<MixPortalPageRole> entity)
        {
            entity.HasKey(e => new { e.RoleId, e.PageId })
                    .HasName("PRIMARY");

            entity.ToTable("mix_portal_page_role");

            entity.HasIndex(e => e.PageId);

            entity.Property(e => e.RoleId)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

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

            entity.HasOne(d => d.Page)
                .WithMany(p => p.MixPortalPageRole)
                .HasForeignKey(d => d.PageId)
                .HasConstraintName("FK_mix_portal_page_role_mix_portal_page");
        }
    }
}
