using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.SQLITE.Base;

namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class MixDataContentConfiguration : SqliteMultiLanguageSEOContentBaseConfiguration<MixDataContent, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixDataContent> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Specificulture)
                .HasColumnType($"{Config.NString}{Config.SmallLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }
    }
}
