using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.SQLITE
{
    public class MixModuleConfiguration : SiteEntityUniqueNameBaseConfiguration<MixModule, int>
    {
        public override void Configure(EntityTypeBuilder<MixModule> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Type)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixModuleType>())
               .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.SmallLength}")
               .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
