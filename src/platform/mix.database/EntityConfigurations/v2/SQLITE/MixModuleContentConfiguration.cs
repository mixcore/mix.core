using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLITE.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.SQLITE
{
    public class MixModuleContentConfiguration : MultilanguageSEOContentBaseConfiguration<MixModuleContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixModuleContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ClassName)
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.String}{SqliteDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet);
            
            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixModuleType>())
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.String}{SqliteDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
