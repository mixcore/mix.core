using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixThemeConfiguration : TenantEntityBaseConfiguration<MixTheme, int>
    {
        public MixThemeConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

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
