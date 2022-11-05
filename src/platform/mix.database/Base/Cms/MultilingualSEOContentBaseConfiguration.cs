using Mix.Database.Services;

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
