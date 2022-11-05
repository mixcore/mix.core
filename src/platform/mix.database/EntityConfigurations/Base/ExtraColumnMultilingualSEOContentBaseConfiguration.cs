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

        }

    }
}
