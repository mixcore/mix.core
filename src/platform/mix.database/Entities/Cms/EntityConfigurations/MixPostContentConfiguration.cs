using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixPostContentConfiguration : MultilingualSEOContentBaseConfiguration<MixPostContent, int>
    {
        public MixPostContentConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixPostContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ClassName)
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}
