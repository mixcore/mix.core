﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.MSSQL
{
    public class MixPortalPageNavigationConfiguration : IEntityTypeConfiguration<MixPortalPageNavigation>
    {
        public void Configure(EntityTypeBuilder<MixPortalPageNavigation> entity)
        {
            entity.ToTable("mix_portal_page_navigation");

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.HasIndex(e => e.PageId)
                .HasDatabaseName("FK_mix_portal_page_navigation_mix_portal_page");

            entity.HasIndex(e => e.ParentId);

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

            entity.HasOne(d => d.Page)
                .WithMany(p => p.MixPortalPageNavigationPage)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mix_portal_page_navigation_mix_portal_page");

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.MixPortalPageNavigationParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mix_portal_page_navigation_mix_portal_page1");
        }
    }
}
