using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Compliance.EntityConfigurations
{
    public class BreakGlassAccessConfiguration : TenantEntityBaseConfiguration<BreakGlassAccess, int>
    {
        public BreakGlassAccessConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<BreakGlassAccess> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("mix_break_glass_access");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnName("user_id")
                .HasColumnType(Config.Guid);

            builder.Property(e => e.Reason)
                .IsRequired()
                .HasColumnName("reason")
                .HasColumnType($"{Config.String}(200)");

            builder.Property(e => e.Justification)
                .IsRequired()
                .HasColumnName("justification")
                .HasColumnType($"{Config.String}(1000)");

            builder.Property(e => e.RequestedAt)
                .HasColumnName("requested_at")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.ApprovedBy)
                .HasColumnName("approved_by")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.ApprovedAt)
                .HasColumnName("approved_at")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.RevokedBy)
                .HasColumnName("revoked_by")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.RevokedAt)
                .HasColumnName("revoked_at")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.ExpiresAt)
                .HasColumnName("expires_at")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasColumnType($"{Config.String}(20)");

            builder.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasColumnType(Config.Boolean);

            // Indexes for efficient querying
            builder.HasIndex(e => new { e.TenantId, e.UserId, e.IsActive })
                .HasDatabaseName("IX_BreakGlassAccess_Tenant_User_Active");

            builder.HasIndex(e => e.ExpiresAt)
                .HasDatabaseName("IX_BreakGlassAccess_ExpiresAt");

            builder.HasIndex(e => e.RequestedAt)
                .HasDatabaseName("IX_BreakGlassAccess_RequestedAt");
        }
    }
}