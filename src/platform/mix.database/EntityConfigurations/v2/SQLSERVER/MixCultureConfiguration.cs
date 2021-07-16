using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLSERVER.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixCultureConfiguration : SqlServerSiteEntityBaseConfiguration<MixCulture, int>
    {
        public override void Configure(EntityTypeBuilder<MixCulture> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Alias)
               .HasColumnType($"{_config.NString}{_config.MediumLength}")
               .HasCharSet(_config.CharSet)
               .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Icon)
               .HasColumnType($"{_config.NString}{_config.MaxLength}")
               .HasCharSet(_config.CharSet);

            builder.Property(e => e.Lcid)
               .HasColumnType($"{_config.NString}{_config.SmallLength}")
               .HasCharSet(_config.CharSet)
               .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Specificulture)
               .HasColumnType($"{_config.NString}{_config.SmallLength}")
               .HasCharSet(_config.CharSet)
               .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Lcid)
               .HasColumnType($"{_config.NString}{_config.MediumLength}")
               .HasCharSet(_config.CharSet)
               .UseCollation(_config.DatabaseCollation);
        }
    }
}
