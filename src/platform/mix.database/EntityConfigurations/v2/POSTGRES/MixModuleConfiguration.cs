using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixModuleConfiguration : SiteEntityUniqueNameBaseConfiguration<MixModule, int>
    {
        public override void Configure(EntityTypeBuilder<MixModule> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Type)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixModuleType>())
               .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
