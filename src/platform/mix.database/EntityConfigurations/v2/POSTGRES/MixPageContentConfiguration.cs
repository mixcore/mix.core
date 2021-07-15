using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixPageContentConfiguration : MultilanguageSEOContentBaseConfiguration<MixPageContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixPageContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ClassName)
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.String}{PostgresSqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet);
            
            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixPageType>())
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.String}{PostgresSqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
