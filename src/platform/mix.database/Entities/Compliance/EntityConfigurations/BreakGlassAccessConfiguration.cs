using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Base.Cms;
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

            builder.Property(e => e.AccessReason)
                .IsRequired()
                .HasColumnName("access_reason")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.Justification)
                .IsRequired()
                .HasColumnName("justification")
                .HasColumnType($"{Config.String}(500)");

            builder.Property(e => e.ApprovedBy)
                .IsRequired()
                .HasColumnName("approved_by")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.AccessStartTime)
                .HasColumnName("access_start_time")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.AccessEndTime)
                .HasColumnName("access_end_time")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.ActualEndTime)
                .HasColumnName("actual_end_time")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasConversion<int>() // Convert enum to int
                .HasColumnType(Config.Integer);

            builder.Property(e => e.IpAddress)
                .HasColumnName("ip_address")
                .HasColumnType($"{Config.String}(45)");

            builder.Property(e => e.UserAgent)
                .HasColumnName("user_agent")
                .HasColumnType($"{Config.String}(500)");

            // Navigation property configuration
            builder.HasMany(e => e.AuditTrail)
                .WithOne(a => a.BreakGlassAccess)
                .HasForeignKey(a => a.BreakGlassAccessId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for efficient querying
            builder.HasIndex(e => new { e.TenantId, e.UserId, e.Status })
                .HasDatabaseName("IX_BreakGlassAccess_Tenant_User_Status");

            builder.HasIndex(e => e.AccessStartTime)
                .HasDatabaseName("IX_BreakGlassAccess_AccessStartTime");

            builder.HasIndex(e => e.AccessEndTime)
                .HasDatabaseName("IX_BreakGlassAccess_AccessEndTime");

            builder.HasIndex(e => e.Status)
                .HasDatabaseName("IX_BreakGlassAccess_Status");
        }
    }
}