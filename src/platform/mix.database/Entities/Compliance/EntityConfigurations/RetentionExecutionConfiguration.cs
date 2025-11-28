using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Compliance.EntityConfigurations
{
    public class RetentionExecutionConfiguration : TenantEntityBaseConfiguration<RetentionExecution, int>
    {
        public RetentionExecutionConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<RetentionExecution> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("mix_retention_execution");

            builder.Property(e => e.RetentionPolicyId)
                .HasColumnName("retention_policy_id")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.ExecutedUtc)
                .HasColumnName("executed_utc")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.ProcessedCount)
                .HasColumnName("processed_count")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.ErrorCount)
                .HasColumnName("error_count")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.ErrorDetails)
                .HasColumnName("error_details")
                .HasColumnType($"{Config.String}(2000)");

            builder.Property(e => e.Success)
                .HasColumnName("success")
                .HasColumnType(Config.Boolean);

            builder.Property(e => e.ExecutedBy)
                .IsRequired()
                .HasColumnName("executed_by")
                .HasColumnType($"{Config.String}(100)");

            // Foreign key relationship
            builder.HasOne(e => e.RetentionPolicy)
                .WithMany(rp => rp.RetentionExecutions)
                .HasForeignKey(e => e.RetentionPolicyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for efficient querying by policy
            builder.HasIndex(e => new { e.TenantId, e.RetentionPolicyId })
                .HasDatabaseName("IX_RetentionExecution_Tenant_Policy");

            // Index for execution date for reporting
            builder.HasIndex(e => e.ExecutedUtc)
                .HasDatabaseName("IX_RetentionExecution_ExecutedUtc");
        }
    }
}