using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.SQLITE
{
    public class MixPostConfiguration : IEntityTypeConfiguration<MixPost>
    {
        public void Configure(EntityTypeBuilder<MixPost> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture })
                    .HasName("PK_mix_post");

            entity.ToTable("mix_post");

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.HasIndex(e => e.Specificulture);

            entity.Property(e => e.Specificulture)
                .HasColumnType("varchar(10)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Excerpt)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.ExtraFields)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.ExtraProperties)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Icon)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Image)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.PublishedDateTime).HasColumnType("datetime");

            entity.Property(e => e.SeoDescription)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.SeoKeywords)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.SeoName)
                .HasColumnType("varchar(500)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.SeoTitle)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Source)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Tags)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Template)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Thumbnail)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Title)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Type)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.HasOne(d => d.SpecificultureNavigation)
                .WithMany(p => p.MixPost)
                .HasPrincipalKey(p => p.Specificulture)
                .HasForeignKey(d => d.Specificulture)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mix_Post_Mix_Culture");

            entity.Property(e => e.EditorValue)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.EditorType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixEditorType>())
               .HasDefaultValue(MixEditorType.Html)
               .HasColumnType("varchar(50)")
               .HasCharSet("utf8")
               .UseCollation("NOCASE");
        }
    }
}