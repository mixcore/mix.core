using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixSiteConfiguration : MySqlEntityBaseConfiguration<MixSite, int>
    {
        public override void Configure(EntityTypeBuilder<MixSite> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{_config.NString}{_config.MediumLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{_config.NString}{_config.MediumLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Description)
                .HasColumnType($"{_config.NString}{_config.MaxLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);
        }
    }
}
