using Mix.Database.Base.Cms;
using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixModuleDataConfiguration : MultilingualSEOContentBaseConfiguration<MixModuleData, int>

    {
        public MixModuleDataConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixModuleData> builder)
        {
            base.Configure(builder);
            
            builder.Property(e => e.SimpleDataColumns)
              .HasColumnType($"{Config.NString}{Config.MaxLength}")
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);
            
            builder.Property(e => e.Value)
              .HasColumnType($"{Config.NString}{Config.MaxLength}")
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);
        }
    }
}
