namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixConfigurationContentConfiguration<TConfig> : MultilingualUniqueNameContentBaseConfiguration<MixConfigurationContent, int, TConfig>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixConfigurationContent> builder)
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
