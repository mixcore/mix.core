using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixDatabaseConfiguration : SiteEntityBaseConfiguration<MixDatabase, int>
    {
        public override void Configure(EntityTypeBuilder<MixDatabase> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Type)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDatabaseType>())
               .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
