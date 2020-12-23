using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations
{
    public class MixConfigurationConfiguration : IEntityTypeConfiguration<MixConfiguration>
    {
        public void Configure(EntityTypeBuilder<MixConfiguration> builder)
        {
            builder
           .Property(e => e.DataType)
           .HasConversion(new EnumToStringConverter<MixEnums.MixDataType>())
           .HasColumnType("varchar(50)");
        }
    }
}
