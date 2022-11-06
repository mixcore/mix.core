using Mix.Database.Services;

namespace Mix.Database.Base.Cms
{
    public abstract class MultilingualContentBaseConfiguration<T, TPrimaryKey> : EntityBaseConfiguration<T, TPrimaryKey>
        where TPrimaryKey : IComparable
        where T : MultilingualContentBase<TPrimaryKey>

    {
        protected MultilingualContentBaseConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Specificulture)
                .IsRequired()
                .HasColumnType($"{Config.NString}{Config.SmallLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Icon)
                .IsRequired(false)
                .HasColumnType($"{Config.NString}{Config.SmallLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }

    }
}
