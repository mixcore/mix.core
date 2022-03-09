using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;
using System;

namespace Mix.Database.EntityConfigurations.Base
{
    public abstract class ExtraColumnMultilanguageSEOContentBaseConfiguration<T, TPrimaryKey, TConfig>
        : MultiLanguageSEOContentBaseConfiguration<T, TPrimaryKey, TConfig>
        where TPrimaryKey : IComparable
        where T : ExtraColumnMultilanguageSEOContentBase<TPrimaryKey>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseName)
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

        }

    }
}
