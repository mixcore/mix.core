using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.MySQL
{
    public class MixPortalPageConfiguration : IEntityTypeConfiguration<MixPortalPage>
    {
        public void Configure(EntityTypeBuilder<MixPortalPage> entity)
        {
            entity.ToTable("mix_portal_page");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Description)
                .HasColumnType("varchar(450)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.Property(e => e.Icon)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.Property(e => e.TextDefault)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.Property(e => e.TextKeyword)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.Property(e => e.Url)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");
        }
    }
}