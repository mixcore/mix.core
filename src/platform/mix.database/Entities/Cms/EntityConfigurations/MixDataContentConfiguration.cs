using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixDataContentConfiguration : MultilingualSEOContentBaseConfiguration<MixDataContent, Guid>
        
    {
        public MixDataContentConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

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
