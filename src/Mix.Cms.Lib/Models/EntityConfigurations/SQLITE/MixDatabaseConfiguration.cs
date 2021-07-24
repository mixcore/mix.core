using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.SQLITE
{
    public class MixDatabaseConfiguration : IEntityTypeConfiguration<MixDatabase>
    {
        public void Configure(EntityTypeBuilder<MixDatabase> entity)
        {
            entity.ToTable("mix_database");

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.Description)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.EdmAutoSend).HasColumnType("bit");

            entity.Property(e => e.EdmFrom)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.EdmSubject)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.EdmTemplate)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.FormTemplate)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");

            entity.Property(e => e.Type)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixDatabaseType>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("NOCASE");
        }
    }
}