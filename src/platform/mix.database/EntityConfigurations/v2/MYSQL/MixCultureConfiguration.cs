using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixCultureConfiguration : SiteEntityBaseConfiguration<MixCulture, int>
    {
        public override void Configure(EntityTypeBuilder<MixCulture> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Alias)
               .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
               .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
               .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Icon)
               .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MaxLength}")
               .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet);

            builder.Property(e => e.Lcid)
               .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
               .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Specificulture)
               .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
               .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Lcid)
               .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
               .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
               .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
