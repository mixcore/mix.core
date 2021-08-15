using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.POSTGRES.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class MixUrlAliasContentConfiguration : PostgresEntityBaseConfiguration<MixUrlAliasContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixUrlAliasContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SourceId)
                .HasColumnType($"{Config.NString}{Config.SmallLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Alias)
                .IsRequired()
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Type)
                .HasConversion(new EnumToStringConverter<MixUrlAliasType>())
                .HasColumnType($"{Config.NString}{Config.SmallLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }
    }
}
