using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;
using System;

namespace Mix.Database.EntityConfigurations.Base
{
    public abstract class MultilanguageUniqueNameContentBaseConfiguration<T, TPrimaryKey, TConfig>
        : MultilanguageContentBaseConfiguration<T, TPrimaryKey, TConfig>
        where TPrimaryKey : IComparable
        where T : MultilanguageUniqueNameContentBase<TPrimaryKey>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Description)
                .HasColumnType($"{Config.NString}{Config.MaxLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Content)
                .HasColumnType($"{Config.NString}{Config.MaxLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }

    }
}
