namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixPostContentConfiguration<TConfig> : MultilingualSEOContentBaseConfiguration<MixPostContent, int, TConfig>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixPostContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ClassName)
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}
