using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.POSTGRES.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class MixThemeConfiguration : PostgresTenantEntityBaseConfiguration<MixTheme, int>
    {
        public override void Configure(EntityTypeBuilder<MixTheme> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.PreviewUrl)
               .IsRequired()
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);
        }
    }
}
