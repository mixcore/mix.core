using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixUrlAliasContentConfiguration : EntityBaseConfiguration<MixUrlAliasContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixUrlAliasContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SourceId)
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Alias)
                .IsRequired()
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Type)
                .HasConversion(new EnumToStringConverter<MixUrlAliasType>())
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
