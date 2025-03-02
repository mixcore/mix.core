using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

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
            builder.Property(e => e.MixModuleContentId)
             .HasColumnName("mix_module_content_id");

            builder.Property(e => e.SimpleDataColumns)
              .HasColumnName("simple_data_columns")
              .HasColumnType($"{Config.NString}{Config.MaxLength}")
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);
            
            builder.Property(e => e.Value)
                .HasColumnName("value")
              .HasColumnType($"{Config.NString}{Config.MaxLength}")
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);

            builder.HasOne(e => e.MixModuleContent)
                .WithMany(e => e.MixModuleDatas)
                .HasForeignKey(m => m.ParentId);
        }
    }
}
