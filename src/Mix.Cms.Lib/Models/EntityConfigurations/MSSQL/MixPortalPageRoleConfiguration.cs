﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.MSSQL
{
    public class MixPortalPageRoleConfiguration : IEntityTypeConfiguration<MixRelatedPost>
    {
        public void Configure(EntityTypeBuilder<MixRelatedPost> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture })
                    .HasName("PK_mix_related_post");

            entity.ToTable("mix_related_post");

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.HasIndex(e => new { e.DestinationId, e.Specificulture });

            entity.HasIndex(e => new { e.SourceId, e.Specificulture });

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
                .HasColumnType("varchar(450)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Image)
                .HasColumnType("varchar(450)")
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

            entity.HasOne(d => d.MixPost)
                .WithMany(p => p.MixRelatedPostMixPost)
                .HasForeignKey(d => new { d.DestinationId, d.Specificulture })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mix_related_post_mix_post1");

            entity.HasOne(d => d.S)
                .WithMany(p => p.MixRelatedPostS)
                .HasForeignKey(d => new { d.SourceId, d.Specificulture })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mix_related_post_mix_post");
        }
    }
}
