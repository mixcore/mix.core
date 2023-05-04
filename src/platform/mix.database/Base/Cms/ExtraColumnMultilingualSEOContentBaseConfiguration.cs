using Mix.Database.Services;

namespace Mix.Database.Base.Cms
{
    public abstract class ExtraColumnMultilingualSEOContentBaseConfiguration<T, TPrimaryKey>
        : MultilingualSEOContentBaseConfiguration<T, TPrimaryKey>
        where TPrimaryKey : IComparable
        where T : ExtraColumnMultilingualSEOContentBase<TPrimaryKey>

    {
        protected ExtraColumnMultilingualSEOContentBaseConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseName)
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            //builder.Property(e => e.ExtraData)
            //.HasConversion(
            //    v => v.ToString(Newtonsoft.Json.Formatting.None),
            //    v => JObject.Parse(v ?? "{}"));
        }

    }
}
