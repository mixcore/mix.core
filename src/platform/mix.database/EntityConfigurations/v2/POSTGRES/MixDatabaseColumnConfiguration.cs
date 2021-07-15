using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixDatabaseColumnConfiguration : EntityBaseConfiguration<MixDatabaseColumn, int>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseColumn> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.MixDatabaseName)
                .IsRequired()
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.Configurations)
                .HasColumnType(PostgresSqlDatabaseConstants.DataTypes.Text)
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.DefaultValue)
                .HasColumnType(PostgresSqlDatabaseConstants.DataTypes.Text)
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
