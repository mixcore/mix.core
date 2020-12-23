using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations
{
    public class MixModuleConfiguration : IEntityTypeConfiguration<MixModule>
    {
        public void Configure(EntityTypeBuilder<MixModule> builder)
        {
            builder
           .Property(e => e.Status)
           .HasConversion(new EnumToStringConverter<MixEnums.MixContentStatus>())
           .HasColumnType("varchar(50)");
        }
    }
}
