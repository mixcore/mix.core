using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Shared.Enums;
using Mix.Lib.Entities.Cms;

namespace Mix.Lib.Entities.EntityConfigurations.MySQL
{
    public class MixPostAssociationConfiguration : IEntityTypeConfiguration<MixPostAssociation>
    {
        public void Configure(EntityTypeBuilder<MixPostAssociation> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture })
                    .HasName("PRIMARY");

            entity.ToTable("mix_post_association");

            entity.HasIndex(e => new { e.DestinationId, e.Specificulture });

            entity.HasIndex(e => new { e.SourceId, e.Specificulture });

            entity.Property(e => e.Specificulture)
                .HasColumnType("varchar(10)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Description)
                .HasColumnType("varchar(450)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.Property(e => e.Image)
                .HasColumnType("varchar(450)")
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
        }
    }
}