using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.POSTGRESQL
{
    public class MixPageModuleConfiguration : IEntityTypeConfiguration<MixPageModule>
    {
        public void Configure(EntityTypeBuilder<MixPageModule> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture })
                    .HasName("PK_mix_page_module");

            entity.ToTable("mix_page_module");

            entity.HasIndex(e => new { e.ModuleId, e.Specificulture });

            entity.HasIndex(e => new { e.PageId, e.Specificulture });

            entity.Property(e => e.Specificulture)
                .HasColumnType("varchar(10)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

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

            entity.HasOne(d => d.MixModule)
                .WithMany(p => p.MixPageModule)
                .HasForeignKey(d => new { d.ModuleId, d.Specificulture })
                .HasConstraintName("FK_Mix_Menu_Module_Mix_Module1");

            entity.HasOne(d => d.MixPage)
                .WithMany(p => p.MixPageModule)
                .HasForeignKey(d => new { d.PageId, d.Specificulture })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mix_Page_Module_Mix_Page");
        }
    }
}