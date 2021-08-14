using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.SQLITE.Base;
using System;

namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class MixDataConfiguration : SqliteEntityBaseConfiguration<MixData, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixData> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseName)
              .HasColumnType($"{_config.NString}{_config.MediumLength}")
              .HasCharSet(_config.CharSet)
              .UseCollation(_config.DatabaseCollation);
        }
    }
}
