using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Shared.Enums;
using Mix.Lib.Entities.Cms;

namespace Mix.Lib.Entities.EntityConfigurations.POSTGRESQL
{
    public class MixPortalPageNavigationConfiguration : IEntityTypeConfiguration<MixPortalPageNavigation>
    {
        public void Configure(EntityTypeBuilder<MixPortalPageNavigation> entity)
        {
            entity.ToTable("mix_portal_page_navigation");

            entity.HasIndex(e => e.PageId)
                .HasDatabaseName("FK_mix_portal_page_navigation_mix_portal_page");

            entity.HasIndex(e => e.ParentId);

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.CreatedDateTime).HasColumnType("timestamp without time zone");

            entity.Property(e => e.Description)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.Image)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.LastModified).HasColumnType("timestamp without time zone");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");
        }
    }
}