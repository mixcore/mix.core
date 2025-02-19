using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class MixMetadataConfiguration : EntityBaseConfiguration<MixMetadata, int>
    {
        public MixMetadataConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixMetadata> builder)
        {
            base.Configure(builder);
            builder.ToTable(MixDbDatabaseNames.Metadata);
            builder.Property(e => e.TenantId)
                .HasColumnName("tenant_id");
            builder.Property(e => e.MixMetadataId)
                .HasColumnName("mix_metadata_id");
            builder.Property(e => e.Type)
                .HasColumnName("type");
            builder.Property(e => e.Content)
                .HasColumnName("content");
            builder.Property(e => e.SeoContent)
                .HasColumnName("seo_content");
        }
    }
}
