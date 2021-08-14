using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.MYSQL.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MixUrlAliasContentConfiguration : MySqlEntityBaseConfiguration<MixUrlAliasContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixUrlAliasContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SourceId)
                .HasColumnType($"{_config.NString}{_config.SmallLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Alias)
                .IsRequired()
                .HasColumnType($"{_config.NString}{_config.MediumLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Type)
                .HasConversion(new EnumToStringConverter<MixUrlAliasType>())
                .HasColumnType($"{_config.NString}{_config.SmallLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);
        }
    }
}
