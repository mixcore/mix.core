namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixLanguageContentConfiguration<TConfig> : MultilingualUniqueNameContentBaseConfiguration<MixLanguageContent, int, TConfig>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixLanguageContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DefaultContent)
                .IsRequired()
                .HasColumnType($"{Config.NString}{Config.MaxLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }
    }
}
