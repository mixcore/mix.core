using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Lib.Models.EntityConfigurations
{
    public class MixPostConfiguration : IEntityTypeConfiguration<MixPost>
    {
        public void Configure(EntityTypeBuilder<MixPost> builder)
        {
            builder
           .Property(e => e.Status)
           .HasConversion(new EnumToStringConverter<MixEnums.MixContentStatus>())
           .HasColumnType("varchar(50)");
        }
    }
}
