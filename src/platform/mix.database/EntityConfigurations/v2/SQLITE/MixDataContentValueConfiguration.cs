using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLITE.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Database.EntityConfigurations.v2.SQLITE
{
    public class MixDataContentValueConfiguration : EntityBaseConfiguration<MixDataContentValue, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixDataContentValue> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseColumnName)
              .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.MixDatabaseName)
              .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet);

            builder.Property(e => e.DateTimeValue)
              .HasColumnType(SqliteDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.StringValue)
              .HasColumnType(SqliteDatabaseConstants.DataTypes.Text)
              .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptValue)
              .HasColumnType(SqliteDatabaseConstants.DataTypes.Text)
              .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptKey)
              .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MaxLength}")
              .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.EncryptType)
              .HasConversion(new EnumToStringConverter<MixEncryptType>())
              .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.SmallLength}")
              .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
