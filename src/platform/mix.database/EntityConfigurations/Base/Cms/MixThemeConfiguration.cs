namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixThemeConfiguration<TConfig> : TenantEntityBaseConfiguration<MixTheme, int, TConfig>
        where TConfig: IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixTheme> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.PreviewUrl)
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);
        }
    }
}
