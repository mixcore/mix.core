using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Compliance.EntityConfigurations
{
    public class DsrRequestConfiguration : TenantEntityBaseConfiguration<DsrRequest, int>
    {
        public DsrRequestConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<DsrRequest> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("mix_dsr_request");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnName("user_id")
                .HasColumnType(Config.Guid);

            builder.Property(e => e.RequestType)
                .IsRequired()
                .HasColumnName("request_type")
                .HasConversion<int>() // Convert enum to int
                .HasColumnType(Config.Integer);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasConversion<int>() // Convert enum to int
                .HasColumnType(Config.Integer)
                .HasDefaultValue(DsrRequestStatus.Pending);

            builder.Property(e => e.SubmittedUtc)
                .HasColumnName("submitted_utc")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.DueUtc)
                .HasColumnName("due_utc")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.ProcessedUtc)
                .HasColumnName("processed_utc")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.SlaMetricMet)
                .HasColumnName("sla_met")
                .HasColumnType(Config.Boolean);

            builder.Property(e => e.Notes)
                .HasColumnName("notes")
                .HasColumnType($"{Config.String}(1000)");

            builder.Property(e => e.ProcessedBy)
                .HasColumnName("processed_by")
                .HasColumnType($"{Config.String}(500)");

            builder.Property(e => e.ExportFilePath)
                .HasColumnName("export_file_path")
                .HasColumnType($"{Config.String}(1000)");

            // Index for efficient querying by user
            builder.HasIndex(e => new { e.TenantId, e.UserId })
                .HasDatabaseName("IX_DsrRequest_Tenant_User");

            // Index for SLA monitoring
            builder.HasIndex(e => new { e.Status, e.DueUtc })
                .HasDatabaseName("IX_DsrRequest_Status_Due");

            // Index for submitted date for retention queries
            builder.HasIndex(e => e.SubmittedUtc)
                .HasDatabaseName("IX_DsrRequest_SubmittedUtc");
        }
    }
}