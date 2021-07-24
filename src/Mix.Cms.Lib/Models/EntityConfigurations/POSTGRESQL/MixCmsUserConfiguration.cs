using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations.POSTGRESQL
{
    public class MixCmsUserConfiguration : IEntityTypeConfiguration<MixCmsUser>
    {
        public void Configure(EntityTypeBuilder<MixCmsUser> entity)
        {
            entity.ToTable("mix_cms_user");

            entity.Property(e => e.Id)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.Address)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.Avatar)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.CreatedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.CreatedDateTime).HasColumnType("timestamp without time zone");

            entity.Property(e => e.Email)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.FirstName)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.LastModified).HasColumnType("timestamp without time zone");

            entity.Property(e => e.LastName)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.MiddleName)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.ModifiedBy)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.PhoneNumber)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixUserStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

            entity.Property(e => e.Username)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");
        }
    }
}