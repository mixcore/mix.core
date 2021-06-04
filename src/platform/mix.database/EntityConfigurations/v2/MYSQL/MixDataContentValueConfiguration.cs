using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixDataContentValueConfiguration : EntityBaseConfiguration<MixDataContentValue, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixDataContentValue> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseColumnName)
              .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.MixDatabaseName)
              .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet);

            builder.Property(e => e.DateTimeValue)
              .HasColumnType(MySqlDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.StringValue)
              .HasColumnType(MySqlDatabaseConstants.DataTypes.Text)
              .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptValue)
              .HasColumnType(MySqlDatabaseConstants.DataTypes.Text)
              .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptKey)
              .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MaxLength}")
              .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptType)
              .HasConversion(new EnumToStringConverter<MixEncryptType>())
              .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
              .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
