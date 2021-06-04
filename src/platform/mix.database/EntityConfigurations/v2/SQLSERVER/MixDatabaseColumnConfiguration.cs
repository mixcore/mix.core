using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLSERVER.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixDatabaseColumnConfiguration : EntityBaseConfiguration<MixDatabaseColumn, int>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseColumn> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.MixDatabaseName)
                .IsRequired()
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.Configurations)
                .IsRequired()
                .HasColumnType(DataTypes.Text)
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.SmallLength}")
               .HasCharSet(DatabaseConfiguration.CharSet);
        }
    }
}
