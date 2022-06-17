using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.SQLITE.Base;

namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class MixContributorConfiguration : SqliteEntityBaseConfiguration<MixContributor, int>
    {
        public override void Configure(EntityTypeBuilder<MixContributor> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.ContentType)
             .IsRequired()
             .HasConversion(new EnumToStringConverter<MixContentType>())
             .HasColumnType($"{Config.NString}{Config.SmallLength}")
             .HasCharSet(Config.CharSet);
        }
    }
}
