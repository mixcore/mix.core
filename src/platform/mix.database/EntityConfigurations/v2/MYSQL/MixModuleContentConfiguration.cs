using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixModuleContentConfiguration : MultilanguageSEOContentBaseConfiguration<MixModuleContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixModuleContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SystemName)
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.String}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet);
            
            builder.Property(e => e.ClassName)
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.String}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet);
            
            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixModuleType>())
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.String}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
