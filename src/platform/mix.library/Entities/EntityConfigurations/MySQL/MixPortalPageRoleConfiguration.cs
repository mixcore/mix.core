using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Shared.Enums;
using Mix.Lib.Entities.Cms;

namespace Mix.Lib.Entities.EntityConfigurations.MySQL
{
    public class MixPortalPageRoleConfiguration : IEntityTypeConfiguration<MixPortalPageRole>
    {
        public void Configure(EntityTypeBuilder<MixPortalPageRole> entity)
        {
            entity.HasKey(e => new { e.Id })
                    .HasName("PK_mix_portal_page_role");

            entity.ToTable("mix_portal_page_role");

            entity.Property(e => e.RoleId).HasMaxLength(50);

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasMaxLength(50)
                .IsUnicode(false);

        }
    }
}