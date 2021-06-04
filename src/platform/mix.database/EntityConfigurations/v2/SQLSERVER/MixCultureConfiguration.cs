using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLSERVER.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixCultureConfiguration : SiteEntityBaseConfiguration<MixCulture, int>
    {
        public override void Configure(EntityTypeBuilder<MixCulture> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Alias)
               .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
               .HasCharSet(DatabaseConfiguration.CharSet)
               .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Icon)
               .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MaxLength}")
               .HasCharSet(DatabaseConfiguration.CharSet);

            builder.Property(e => e.Lcid)
               .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.SmallLength}")
               .HasCharSet(DatabaseConfiguration.CharSet)
               .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Specificulture)
               .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.SmallLength}")
               .HasCharSet(DatabaseConfiguration.CharSet)
               .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Lcid)
               .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
               .HasCharSet(DatabaseConfiguration.CharSet)
               .UseCollation(DatabaseConfiguration.DatabaseCollation);
        }
    }
}
