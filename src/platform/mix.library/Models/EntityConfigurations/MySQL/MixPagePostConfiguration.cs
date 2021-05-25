using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Lib.Enums;
using Mix.Lib.Models.Cms;

namespace Mix.Lib.Models.EntityConfigurations.MySQL
{
    public class MixPagePostConfiguration : IEntityTypeConfiguration<MixPagePost>
    {
        public void Configure(EntityTypeBuilder<MixPagePost> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture })
                    .HasName("PRIMARY");

            entity.ToTable("mix_page_post");

            entity.HasIndex(e => new { e.PageId, e.Specificulture });

            entity.HasIndex(e => new { e.PostId, e.Specificulture });

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
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.Image)
                .HasColumnType("varchar(250)")
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

            entity.HasOne(d => d.MixPage)
                .WithMany(p => p.MixPagePost)
                .HasForeignKey(d => new { d.PageId, d.Specificulture })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mix_Page_Post_Mix_Page");

            entity.HasOne(d => d.MixPost)
                .WithMany(p => p.MixPagePost)
                .HasForeignKey(d => new { d.PostId, d.Specificulture })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mix_Page_Post_Mix_Post");
        }
    }
}