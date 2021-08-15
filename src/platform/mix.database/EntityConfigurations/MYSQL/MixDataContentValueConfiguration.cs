using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.MYSQL.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MixDataContentValueConfiguration : MySqlEntityBaseConfiguration<MixDataContentValue, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixDataContentValue> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseColumnName)
              .HasColumnType($"{Config.NString}{Config.MediumLength}")
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);
            
            builder.Property(e => e.MixDatabaseName)
              .HasColumnType($"{Config.NString}{Config.MediumLength}")
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);

            builder.Property(e => e.DateTimeValue)
              .HasColumnType(Config.DateTime);

            builder.Property(e => e.StringValue)
              .HasColumnType(Config.Text)
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);
            
            builder.Property(e => e.EncryptValue)
              .HasColumnType(Config.Text)
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);
            
            builder.Property(e => e.EncryptKey)
              .HasColumnType($"{Config.NString}{Config.MaxLength}")
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);
            
            builder.Property(e => e.EncryptType)
              .HasConversion(new EnumToStringConverter<MixEncryptType>())
              .HasColumnType($"{Config.NString}{Config.SmallLength}")
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);
        }
    }
}
