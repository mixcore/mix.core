using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Cms;

using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.EntityConfigurations.MSSQL
{
    public class MixDatabaseContentAssociationConfiguration : IEntityTypeConfiguration<MixDatabaseContentAssociation>
    {
        public void Configure(EntityTypeBuilder<MixDatabaseContentAssociation> entity)
        {
            entity.HasKey(e => new { e.Id, e.Specificulture })
                    .HasName("PK_mix_related_attribute_data");

            entity.ToTable("mix_related_attribute_data");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Specificulture)
                .HasColumnType("varchar(10)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.AttributeSetName)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.Property(e => e.DataId)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Description)
                .HasColumnType("varchar(450)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.LastModified).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.ParentId)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.ParentType)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixDatabaseContentAssociationType>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .HasCollation("Vietnamese_CI_AS");
        }
    }
}
