﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.EntityConfigurations.MySQL
{
    public class MixUrlAliasConfiguration : IEntityTypeConfiguration<MixUrlAlias>
    {
        public void Configure(EntityTypeBuilder<MixUrlAlias> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture })
                    .HasName("PRIMARY");

            entity.ToTable("mix_url_alias");

            entity.HasIndex(e => e.Specificulture);

            entity.Property(e => e.Specificulture)
                .HasColumnType("varchar(10)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.Alias)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.SourceId)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.Type)
                .HasConversion(new EnumToStringConverter<MixUrlAliasType>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("utf8_unicode_ci");

            entity.HasOne(d => d.SpecificultureNavigation)
                .WithMany(p => p.MixUrlAlias)
                .HasPrincipalKey(p => p.Specificulture)
                .HasForeignKey(d => d.Specificulture)
                .HasConstraintName("FK_Mix_Url_Alias_Mix_Culture");
        }
    }
}
