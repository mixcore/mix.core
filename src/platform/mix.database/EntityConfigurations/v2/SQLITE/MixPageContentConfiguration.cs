using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLITE.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Database.EntityConfigurations.v2.SQLITE
{
    public class MixPageContentConfiguration : SqliteMultilanguageSEOContentBaseConfiguration<MixPageContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixPageContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ClassName)
                .HasColumnType($"{_config.String}{_config.SmallLength}")
                .HasCharSet(_config.CharSet);
            
            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixPageType>())
                .HasColumnType($"{_config.String}{_config.SmallLength}")
                .HasCharSet(_config.CharSet);
        }
    }
}
