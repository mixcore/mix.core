using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Compliance.EntityConfigurations
{
    public class DataProtectionImpactAssessmentConfiguration : TenantEntityBaseConfiguration<DataProtectionImpactAssessment, int>
    {
        public DataProtectionImpactAssessmentConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<DataProtectionImpactAssessment> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("mix_data_protection_impact_assessment");

            builder.Property(e => e.Title)
                .IsRequired()
                .HasColumnName("title")
                .HasColumnType($"{Config.String}(200)");

            builder.Property(e => e.ProcessingDescription)
                .IsRequired()
                .HasColumnName("processing_description")
                .HasColumnType($"{Config.String}(2000)");

            builder.Property(e => e.DataTypes)
                .IsRequired()
                .HasColumnName("data_types")
                .HasColumnType($"{Config.String}(1000)");

            builder.Property(e => e.DataSubjects)
                .IsRequired()
                .HasColumnName("data_subjects")
                .HasColumnType($"{Config.String}(1000)");

            builder.Property(e => e.LegalBasis)
                .IsRequired()
                .HasColumnName("legal_basis")
                .HasColumnType($"{Config.String}(1000)");

            builder.Property(e => e.RiskScore)
                .HasColumnName("risk_score")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.RiskAssessment)
                .HasColumnName("risk_assessment")
                .HasColumnType($"{Config.String}(2000)");

            builder.Property(e => e.Safeguards)
                .HasColumnName("safeguards")
                .HasColumnType($"{Config.String}(2000)");

            builder.Property(e => e.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasColumnType($"{Config.String}(50)");

            builder.Property(e => e.ApprovedBy)
                .HasColumnName("approved_by")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.ApprovedAt)
                .HasColumnName("approved_at")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.RejectedBy)
                .HasColumnName("rejected_by")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.RejectedAt)
                .HasColumnName("rejected_at")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.RejectionReason)
                .HasColumnName("rejection_reason")
                .HasColumnType($"{Config.String}(1000)");

            builder.Property(e => e.ReviewComments)
                .HasColumnName("review_comments")
                .HasColumnType($"{Config.String}(2000)");

            builder.Property(e => e.ReviewedAt)
                .HasColumnName("reviewed_at")
                .HasColumnType(Config.DateTime);

            // Indexes for efficient querying
            builder.HasIndex(e => new { e.TenantId, e.Status })
                .HasDatabaseName("IX_DPIA_Tenant_Status");

            builder.HasIndex(e => e.RiskScore)
                .HasDatabaseName("IX_DPIA_RiskScore");

            builder.HasIndex(e => e.CreatedDateTime)
                .HasDatabaseName("IX_DPIA_CreatedDateTime");
        }
    }
}