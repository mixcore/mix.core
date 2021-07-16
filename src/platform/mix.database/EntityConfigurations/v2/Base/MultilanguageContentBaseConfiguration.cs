using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.Base
{
    public abstract class MultilanguageContentBaseConfiguration<T, TPrimaryKey, TConfig> : EntityBaseConfiguration<T, TPrimaryKey, TConfig>
        where TPrimaryKey : IComparable
        where T : MultilanguageContentBase<TPrimaryKey>
         where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Specificulture)
                .IsRequired()
                .HasColumnType($"{_config.NString}{_config.SmallLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);
        }

    }
}
