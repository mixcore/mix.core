using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.MySQL
{
    public class MixModuleDataConfiguration : IEntityTypeConfiguration<MixModuleData>
    {
        public void Configure(EntityTypeBuilder<MixModuleData> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture })
                    .HasName("PRIMARY");

            entity.ToTable("mix_module_data");

            entity.HasIndex(e => new { e.ModuleId, e.Specificulture });

            entity.HasIndex(e => new { e.PageId, e.Specificulture });

            entity.HasIndex(e => new { e.PostId, e.Specificulture });

            entity.HasIndex(e => new { e.ModuleId, e.PageId, e.Specificulture });

            entity.Property(e => e.Id)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.Property(e => e.Specificulture)
                .HasColumnType("varchar(10)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Fields)
                .IsRequired()
                .HasColumnType("text")
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

            entity.Property(e => e.Value)
                .HasColumnType("text")
                .HasCharSet("utf8")
                .UseCollation("utf8_unicode_ci");

            entity.HasOne(d => d.MixModule)
                .WithMany(p => p.MixModuleData)
                .HasForeignKey(d => new { d.ModuleId, d.Specificulture })
                .HasConstraintName("FK_Mix_Module_Data_Mix_Module");

            entity.HasOne(d => d.MixPage)
                .WithMany(p => p.MixModuleData)
                .HasForeignKey(d => new { d.PageId, d.Specificulture })
                .HasConstraintName("FK_mix_module_data_mix_page");

            entity.HasOne(d => d.MixPost)
                .WithMany(p => p.MixModuleData)
                .HasForeignKey(d => new { d.PostId, d.Specificulture })
                .HasConstraintName("FK_mix_module_data_mix_post");
        }
    }
}