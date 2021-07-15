using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLSERVER.Base;
using Mix.Database.Extenstions;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixDatabaseColumnConfiguration : EntityBaseConfiguration<MixDatabaseColumn, int>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseColumn> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.MixDatabaseName)
                .IsRequired()
                .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.Configurations)
                .HasColumnType(SqlServerDatabaseConstants.DataTypes.Text)
                .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.DefaultValue)
                .HasColumnType(SqlServerDatabaseConstants.DataTypes.Text)
                .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
