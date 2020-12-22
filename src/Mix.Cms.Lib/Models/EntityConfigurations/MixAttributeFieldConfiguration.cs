using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations
{
    public class MixAttributeFieldConfiguration : IEntityTypeConfiguration<MixAttributeField>
    {
        public void Configure(EntityTypeBuilder<MixAttributeField> builder)
        {
            builder
           .Property(e => e.DataType)
           .HasConversion(new EnumToStringConverter<MixEnums.MixDataType>())
           .HasColumnType("varchar(30)");
        }
    }
}
