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
            builder.ToTable(MixDbDatabaseNames.Metadata);
            builder.Property(e => e.TenantId)
                .HasColumnName("tenant_id");
            base.Configure(builder);
        }
    }
}
