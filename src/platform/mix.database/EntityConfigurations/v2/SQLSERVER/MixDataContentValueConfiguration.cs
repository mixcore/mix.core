using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLSERVER.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixDataContentValueConfiguration : EntityBaseConfiguration<MixDataContentValue, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixDataContentValue> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseColumnName)
              .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.MixDatabaseName)
              .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet);

            builder.Property(e => e.DateTimeValue)
              .HasColumnType(SqlServerDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.StringValue)
              .HasColumnType(SqlServerDatabaseConstants.DataTypes.Text)
              .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptValue)
              .HasColumnType(SqlServerDatabaseConstants.DataTypes.Text)
              .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptKey)
              .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MaxLength}")
              .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptType)
              .HasConversion(new EnumToStringConverter<MixEncryptType>())
              .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.SmallLength}")
              .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
