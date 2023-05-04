namespace Mix.Database.EntityConfigurations.Base
{
    public abstract class ExtraColumnMultilingualSEOContentBaseConfiguration<T, TPrimaryKey, TConfig>
        : MultilingualSEOContentBaseConfiguration<T, TPrimaryKey, TConfig>
        where TPrimaryKey : IComparable
        where T : ExtraColumnMultilingualSEOContentBase<TPrimaryKey>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseName)
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            //builder.Property(e => e.ExtraData)
            //   .HasConversion(
            //       v => v.ToString(Newtonsoft.Json.Formatting.None),
            //       v => JObject.Parse(v ?? "{}"));
        }

    }
}
