namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixDataContentConfiguration<TConfig> : MultilingualSEOContentBaseConfiguration<MixDataContent, Guid, TConfig>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixDataContent> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Specificulture)
                .HasColumnType($"{Config.NString}{Config.SmallLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }
    }
}
