using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixModuleContentConfiguration : PostgresMultilanguageSEOContentBaseConfiguration<MixModuleContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixModuleContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SystemName)
                .HasColumnType($"{_config.String}{_config.MediumLength}")
                .HasCharSet(_config.CharSet);

            builder.Property(e => e.ClassName)
                .HasColumnType($"{_config.String}{_config.SmallLength}")
                .HasCharSet(_config.CharSet);
            
            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixModuleType>())
                .HasColumnType($"{_config.String}{_config.SmallLength}")
                .HasCharSet(_config.CharSet);
        }
    }
}
