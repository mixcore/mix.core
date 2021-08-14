using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;
using System;

namespace Mix.Database.EntityConfigurations.Base
{
    public abstract class MultilanguageSEOContentBaseConfiguration<T, TPrimaryKey, TConfig> : MultilanguageContentBaseConfiguration<T, TPrimaryKey, TConfig>
       where TPrimaryKey : IComparable
       where T : MultilanguageSEOContentBase<TPrimaryKey>
         where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title)
               .IsRequired()
               .HasColumnType($"{_config.NString}{_config.MediumLength}")
               .HasCharSet(_config.CharSet)
               .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Excerpt)
                .HasColumnType($"{_config.NString}{_config.MaxLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);
            
            builder.Property(e => e.Content)
                .HasColumnType($"{_config.Text}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Layout)
              .HasColumnType($"{_config.NString}{_config.MediumLength}")
              .HasCharSet(_config.CharSet)
              .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Template)
              .HasColumnType($"{_config.NString}{_config.MediumLength}")
              .HasCharSet(_config.CharSet)
              .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.MixDatabaseName)
                .HasColumnType($"{_config.NString}{_config.MediumLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.PublishedDateTime)
                .HasColumnType(_config.DateTime);

            builder.Property(e => e.Image)
                .HasColumnType($"{_config.NString}{_config.MediumLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Source)
                .HasColumnType($"{_config.NString}{_config.MediumLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.SeoDescription)
                .HasColumnType($"{_config.NString}{_config.MaxLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.SeoKeywords)
                .HasColumnType($"{_config.NString}{_config.MaxLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.SeoName)
                .HasColumnType($"{_config.NString}{_config.MediumLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.SeoName)
                .HasColumnType($"{_config.NString}{_config.MaxLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

        }

    }
}
