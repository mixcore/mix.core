using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

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

            builder.Property(e => e.MixPostContentId)
              .HasColumnName("mix_post_content_id")
              .HasColumnType(Config.Integer);

            builder.Property(e => e.MixDbId)
              .HasColumnName("mix_db_id")
              .HasColumnType(Config.Integer);

            builder.Property(e => e.ClassName)
                .HasColumnName("class_name")
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);

            builder.Property(e => e.MixDatabaseName)
                .HasColumnName("mix_database_name")
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);

            builder.HasOne(e => e.MixPost)
                .WithMany(e => e.MixPostContents)
                .HasForeignKey(m => m.ParentId);
        }
    }
}
