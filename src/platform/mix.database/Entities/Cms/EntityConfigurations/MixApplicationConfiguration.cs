using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixApplicationConfiguration : TenantEntityBaseConfiguration<MixApplication, int>

    {
        public MixApplicationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixApplication> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.DisplayName)
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
