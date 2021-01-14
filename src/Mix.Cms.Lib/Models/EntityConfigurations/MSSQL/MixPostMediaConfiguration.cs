﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.MSSQL
{
    public class MixPostMediaConfiguration : IEntityTypeConfiguration<MixPostMedia>
    {
        public void Configure(EntityTypeBuilder<MixPostMedia> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture })
                    .HasName("PK_mix_post_media");

            entity.ToTable("mix_post_media");

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.HasIndex(e => new { e.MediaId, e.Specificulture });

            entity.HasIndex(e => new { e.PostId, e.Specificulture });

            entity.Property(e => e.Specificulture)
                .HasColumnType("varchar(10)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Description)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Image)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixEnums.MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.HasOne(d => d.MixMedia)
                .WithMany(p => p.MixPostMedia)
                .HasForeignKey(d => new { d.MediaId, d.Specificulture })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mix_post_media_mix_media");

            entity.HasOne(d => d.MixPost)
                .WithMany(p => p.MixPostMedia)
                .HasForeignKey(d => new { d.PostId, d.Specificulture })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mix_post_media_mix_post");
        }
    }
}
