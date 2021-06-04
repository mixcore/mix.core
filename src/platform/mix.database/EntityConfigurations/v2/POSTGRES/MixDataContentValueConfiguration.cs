using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixDataContentValueConfiguration : EntityBaseConfiguration<MixDataContentValue, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixDataContentValue> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseColumnName)
              .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.MixDatabaseName)
              .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet);

            builder.Property(e => e.DateTimeValue)
              .HasColumnType(PostgresSqlDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.StringValue)
              .HasColumnType(PostgresSqlDatabaseConstants.DataTypes.Text)
              .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptValue)
              .HasColumnType(PostgresSqlDatabaseConstants.DataTypes.Text)
              .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptKey)
              .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MaxLength}")
              .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptType)
              .HasConversion(new EnumToStringConverter<MixEncryptType>())
              .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
              .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
