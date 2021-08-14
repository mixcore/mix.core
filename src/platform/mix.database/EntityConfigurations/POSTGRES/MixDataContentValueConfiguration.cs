using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.POSTGRES.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class MixDataContentValueConfiguration : PostgresEntityBaseConfiguration<MixDataContentValue, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixDataContentValue> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseColumnName)
              .HasColumnType($"{_config.NString}{_config.MediumLength}")
              .HasCharSet(_config.CharSet)
              .UseCollation(_config.DatabaseCollation);
            
            builder.Property(e => e.MixDatabaseName)
              .HasColumnType($"{_config.NString}{_config.MediumLength}")
              .HasCharSet(_config.CharSet)
              .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{_config.NString}{_config.SmallLength}")
               .HasCharSet(_config.CharSet);

            builder.Property(e => e.DateTimeValue)
              .HasColumnType(_config.DateTime);

            builder.Property(e => e.StringValue)
              .HasColumnType(_config.Text)
              .HasCharSet(_config.CharSet)
              .UseCollation(_config.DatabaseCollation);
            
            builder.Property(e => e.EncryptValue)
              .HasColumnType(_config.Text)
              .HasCharSet(_config.CharSet)
              .UseCollation(_config.DatabaseCollation);
            
            builder.Property(e => e.EncryptKey)
              .HasColumnType($"{_config.NString}{_config.MaxLength}")
              .HasCharSet(_config.CharSet)
              .UseCollation(_config.DatabaseCollation);
            
            builder.Property(e => e.EncryptType)
              .HasConversion(new EnumToStringConverter<MixEncryptType>())
              .HasColumnType($"{_config.NString}{_config.SmallLength}")
              .HasCharSet(_config.CharSet)
              .UseCollation(_config.DatabaseCollation);
        }
    }
}
