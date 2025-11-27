using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Compliance.EntityConfigurations
{
    public class RetentionPolicyConfiguration : TenantEntityBaseConfiguration<RetentionPolicy, int>
    {
        public RetentionPolicyConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<RetentionPolicy> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("mix_retention_policy");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.Category)
                .IsRequired()
                .HasColumnName("category")
                .HasColumnType($"{Config.String}(50)");

            builder.Property(e => e.MaxAgeDays)
                .HasColumnName("max_age_days")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.ActionOnExpiry)
                .IsRequired()
                .HasColumnName("action_on_expiry")
                .HasConversion<int>() // Convert enum to int
                .HasColumnType(Config.Integer);

            builder.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasColumnType(Config.Boolean)
                .HasDefaultValue(true);

            // Navigation properties
            builder.HasMany(e => e.DataFields)
                .WithOne(d => d.RetentionPolicy)
                .HasForeignKey(d => d.RetentionPolicyId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.RetentionExecutions)
                .WithOne(r => r.RetentionPolicy)
                .HasForeignKey(r => r.RetentionPolicyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint for name per tenant
            builder.HasIndex(e => new { e.TenantId, e.Name })
                .IsUnique()
                .HasDatabaseName("IX_RetentionPolicy_Tenant_Name");
        }
    }
}