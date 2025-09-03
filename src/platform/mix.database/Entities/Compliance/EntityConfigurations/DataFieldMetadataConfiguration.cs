using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Compliance.EntityConfigurations
{
    public class DataFieldMetadataConfiguration : TenantEntityBaseConfiguration<DataFieldMetadata, int>
    {
        public DataFieldMetadataConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<DataFieldMetadata> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("mix_data_field_metadata");

            builder.Property(e => e.EntityName)
                .IsRequired()
                .HasColumnName("entity_name")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.FieldName)
                .IsRequired()
                .HasColumnName("field_name")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.Classification)
                .IsRequired()
                .HasColumnName("classification")
                .HasConversion<int>() // Convert enum to int
                .HasColumnType(Config.Integer);

            builder.Property(e => e.PurposeId)
                .HasColumnName("purpose_id")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.RetentionPolicyId)
                .HasColumnName("retention_policy_id")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.EncryptionRequired)
                .HasColumnName("encryption_required")
                .HasColumnType(Config.Boolean);

            builder.Property(e => e.LastReviewedUtc)
                .HasColumnName("last_reviewed_utc")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.Notes)
                .HasColumnName("notes")
                .HasColumnType($"{Config.String}(500)");

            // Foreign key relationships
            builder.HasOne(e => e.Purpose)
                .WithMany(p => p.DataFields)
                .HasForeignKey(e => e.PurposeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.RetentionPolicy)
                .WithMany(rp => rp.DataFields)
                .HasForeignKey(e => e.RetentionPolicyId)
                .OnDelete(DeleteBehavior.SetNull);

            // Unique constraint for entity+field per tenant
            builder.HasIndex(e => new { e.TenantId, e.EntityName, e.FieldName })
                .IsUnique()
                .HasDatabaseName("IX_DataFieldMetadata_Tenant_Entity_Field");
        }
    }
}