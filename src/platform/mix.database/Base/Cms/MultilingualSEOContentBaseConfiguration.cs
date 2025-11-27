using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Base.Cms
{
    public abstract class MultilingualSEOContentBaseConfiguration<T, TPrimaryKey> : MultilingualContentBaseConfiguration<T, TPrimaryKey>
       where TPrimaryKey : IComparable
       where T : MultilingualSEOContentBase<TPrimaryKey>

    {
        protected MultilingualSEOContentBaseConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title)
                .HasColumnName("title")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Excerpt)
                .HasColumnName("excerpt")
                .HasColumnType($"{Config.NString}{Config.MaxLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Content)
                .HasColumnName("content")
                .HasColumnType($"{Config.Text}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.LayoutId)
                .HasColumnName("layout_id")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.TemplateId)
                .HasColumnName("template_id")
              .HasColumnType(Config.Integer);

            builder.Property(e => e.PublishedDateTime)
                .HasColumnName("published_date_time")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.Image)
                .HasColumnName("image")
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Source)
                .HasColumnName("source")
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.SeoDescription)
                .HasColumnName("seo_description")
                .HasColumnType($"{Config.NString}{Config.MaxLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.SeoKeywords)
                .HasColumnName("seo_keywords")
                .HasColumnType($"{Config.NString}{Config.MaxLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.SeoName)
                .HasColumnName("seo_name")
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
            
            builder.Property(e => e.SeoTitle)
                .HasColumnName("seo_title")
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }

    }
}
