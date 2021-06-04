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
              .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
              .HasCharSet(DatabaseConfiguration.CharSet)
              .UseCollation(DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.MixDatabaseName)
              .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
              .HasCharSet(DatabaseConfiguration.CharSet)
              .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.SmallLength}")
               .HasCharSet(DatabaseConfiguration.CharSet);

            builder.Property(e => e.DateTimeValue)
              .HasColumnType(DataTypes.DateTime);

            builder.Property(e => e.StringValue)
              .HasColumnType(DataTypes.Text)
              .HasCharSet(DatabaseConfiguration.CharSet)
              .UseCollation(DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptValue)
              .HasColumnType(DataTypes.Text)
              .HasCharSet(DatabaseConfiguration.CharSet)
              .UseCollation(DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptKey)
              .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MaxLength}")
              .HasCharSet(DatabaseConfiguration.CharSet)
              .UseCollation(DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptType)
              .HasConversion(new EnumToStringConverter<MixEncryptType>())
              .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.SmallLength}")
              .HasCharSet(DatabaseConfiguration.CharSet)
              .UseCollation(DatabaseConfiguration.DatabaseCollation);
        }
    }
}
