using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixContributorConfiguration<TConfig> : EntityBaseConfiguration<MixContributor, int, TConfig>
        where TConfig : IDatabaseConstants
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
