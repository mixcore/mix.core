using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.MYSQL.Base;
using Mix.Shared.Enums;
using Mix.Shared.Models;
using Mix.Database.Extenstions;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MixDatabaseColumnConfiguration : MySqlEntityBaseConfiguration<MixDatabaseColumn, int>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseColumn> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{_config.NString}{_config.MediumLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{_config.NString}{_config.MediumLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.MixDatabaseName)
                .IsRequired()
                .HasColumnType($"{_config.NString}{_config.MediumLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);
            
            builder.Property(e => e.Configurations)
                .HasColumnType(_config.Text)
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.DefaultValue)
                .HasColumnType(_config.Text)
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{_config.NString}{_config.SmallLength}")
               .HasCharSet(_config.CharSet);
        }
    }
}
