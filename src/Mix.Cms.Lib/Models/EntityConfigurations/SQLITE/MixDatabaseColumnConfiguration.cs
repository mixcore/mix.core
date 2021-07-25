using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.SQLITE
{
    public class MixDatabaseColumnConfiguration : IEntityTypeConfiguration<MixDatabaseColumn>
    {
        public void Configure(EntityTypeBuilder<MixDatabaseColumn> entity)
        {
            entity.ToTable("mix_database_column");

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType("varchar(50)")
               .HasCharSet("utf8")
               .UseCollation("NOCASE");

            entity.Property(e => e.MixDatabaseName).HasMaxLength(250);

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.DefaultValue).HasColumnType("ntext");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

            entity.Property(e => e.Options).HasColumnType("ntext");

            entity.Property(e => e.Regex).HasMaxLength(4000);

            entity.Property(e => e.Status)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixContentStatus>())
               .HasColumnType("varchar(50)")
               .HasCharSet("utf8")
               .UseCollation("NOCASE");
        }
    }
}