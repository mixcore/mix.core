using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLITE.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLITE
{
    public class MixThemeConfiguration : SiteEntityBaseConfiguration<MixTheme, int>
    {
        public override void Configure(EntityTypeBuilder<MixTheme> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.PreviewUrl)
               .IsRequired()
               .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MediumLength}")
               .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
               .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
