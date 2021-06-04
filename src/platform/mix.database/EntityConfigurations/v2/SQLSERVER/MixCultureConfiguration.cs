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
               .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MediumLength}")
               .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
               .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Icon)
               .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MaxLength}")
               .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet);

            builder.Property(e => e.Lcid)
               .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
               .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Specificulture)
               .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
               .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Lcid)
               .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MediumLength}")
               .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
               .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
