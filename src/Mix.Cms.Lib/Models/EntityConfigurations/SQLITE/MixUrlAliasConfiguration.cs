using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.SQLITE
{
    public class MixUrlAliasConfiguration : IEntityTypeConfiguration<MixUrlAlias>
    {
        public void Configure(EntityTypeBuilder<MixUrlAlias> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture });

            entity.ToTable("mix_url_alias");

            entity.Property(e => e.Id)
            .ValueGeneratedNever();

            entity.Property(e => e.Specificulture)
            .HasColumnType("varchar(10)")
            .HasCharSet("utf8")
            .UseCollation("NOCASE");

            entity.Property(e => e.Alias).HasMaxLength(250);

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Description).HasMaxLength(4000);

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.SourceId).HasMaxLength(250);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            entity.HasOne(d => d.SpecificultureNavigation)
                .WithMany(p => p.MixUrlAlias)
                .HasPrincipalKey(p => p.Specificulture)
                .HasForeignKey(d => d.Specificulture)
                .HasConstraintName("FK_Mix_Url_Alias_Mix_Culture");
        }
    }
}