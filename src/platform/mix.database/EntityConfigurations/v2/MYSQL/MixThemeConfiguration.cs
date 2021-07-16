using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixThemeConfiguration : MySqlSiteEntityBaseConfiguration<MixTheme, int>
    {
        public override void Configure(EntityTypeBuilder<MixTheme> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.PreviewUrl)
               .IsRequired()
               .HasColumnType($"{_config.NString}{_config.MediumLength}")
               .HasCharSet(_config.CharSet)
               .UseCollation(_config.DatabaseCollation);
        }
    }
}
