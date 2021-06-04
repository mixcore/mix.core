using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLITE.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.SQLITE
{
    public class MixDatabaseColumnConfiguration : EntityBaseConfiguration<MixDatabaseColumn, int>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseColumn> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.MixDatabaseName)
                .IsRequired()
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.Configurations)
                .IsRequired()
                .HasColumnType(SqliteDatabaseConstants.DataTypes.Text)
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
