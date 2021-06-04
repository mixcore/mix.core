using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixModuleContentConfiguration : MultilanguageSEOContentBaseConfiguration<MixModuleContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixModuleContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ClassName)
                .IsRequired()
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.String}{PostgresSqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet);
            
            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixModuleType>())
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.String}{PostgresSqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
