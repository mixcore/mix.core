using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;

namespace Mix.Database.EntityConfigurations.Base
{
    public abstract class MultiLanguageSEOContentBaseConfiguration<T, TPrimaryKey, TConfig> : MultiLanguageContentBaseConfiguration<T, TPrimaryKey, TConfig>
       where TPrimaryKey : IComparable
       where T : MultiLanguageSEOContentBase<TPrimaryKey>
         where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title)
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Excerpt)
                .HasColumnType($"{Config.NString}{Config.MaxLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Content)
                .HasColumnType($"{Config.Text}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.LayoutId)
              .HasColumnType(Config.Integer);

            builder.Property(e => e.TemplateId)
              .HasColumnType(Config.Integer);

            builder.Property(e => e.PublishedDateTime)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.Image)
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Source)
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.SeoDescription)
                .HasColumnType($"{Config.NString}{Config.MaxLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.SeoKeywords)
                .HasColumnType($"{Config.NString}{Config.MaxLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.SeoName)
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }

    }
}
