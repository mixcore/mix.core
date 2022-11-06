using Mix.Database.Services;

namespace Mix.Database.Base.Cms
{
    public abstract class MultilingualUniqueNameContentBaseConfiguration<T, TPrimaryKey>
        : MultilingualContentBaseConfiguration<T, TPrimaryKey>
        where TPrimaryKey : IComparable
        where T : MultilingualUniqueNameContentBase<TPrimaryKey>

    {
        protected MultilingualUniqueNameContentBaseConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

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
