using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;
using System;

namespace Mix.Database.EntityConfigurations.Base
{
    public abstract class SiteEntityUniqueNameBaseConfiguration<T, TPrimaryKey, TConfig> : SiteEntityBaseConfiguration<T, TPrimaryKey, TConfig>
        where TPrimaryKey : IComparable
        where T : SiteEntityUniqueNameBase<TPrimaryKey>
        where TConfig: IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SystemName)
               .IsRequired()
               .HasColumnType($"{_config.NString}{_config.MediumLength}")
               .HasCharSet(_config.CharSet)
               .UseCollation(_config.DatabaseCollation);

        }

    }
}
