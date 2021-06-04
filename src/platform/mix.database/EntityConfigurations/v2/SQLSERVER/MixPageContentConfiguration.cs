using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLSERVER.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixPageContentConfiguration : MultilanguageSEOContentBaseConfiguration<MixPageContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixPageContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ClassName)
                .IsRequired()
                .HasColumnType($"{DataTypes.String}{DatabaseConfiguration.SmallLength}")
                .HasCharSet(DatabaseConfiguration.CharSet);
            
            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixPageType>())
                .HasColumnType($"{DataTypes.String}{DatabaseConfiguration.SmallLength}")
                .HasCharSet(DatabaseConfiguration.CharSet);
        }
    }
}
