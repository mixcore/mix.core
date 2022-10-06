namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixApplicationConfiguration<TConfig> : TenantEntityBaseConfiguration<MixApplication, int, TConfig>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixApplication> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Title)
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
            builder.Property(e => e.BaseHref)
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
            builder.Property(e => e.BaseRoute)
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
            builder.Property(e => e.Domain)
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
            builder.Property(e => e.BaseApiUrl)
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
            builder.Property(e => e.MixDatabaseName)
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);

        }
    }
}
