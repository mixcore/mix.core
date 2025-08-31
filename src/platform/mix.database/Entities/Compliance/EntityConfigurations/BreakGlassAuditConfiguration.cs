using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Compliance.EntityConfigurations
{
    public class BreakGlassAuditConfiguration : EntityBaseConfiguration<BreakGlassAudit, int>
    {
        public BreakGlassAuditConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<BreakGlassAudit> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("mix_break_glass_audit");

            builder.Property(e => e.BreakGlassAccessId)
                .IsRequired()
                .HasColumnName("break_glass_access_id")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.Action)
                .IsRequired()
                .HasColumnName("action")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.EntityType)
                .IsRequired()
                .HasColumnName("entity_type")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.EntityId)
                .IsRequired()
                .HasColumnName("entity_id")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.PhiAccessed)
                .HasColumnName("phi_accessed")
                .HasColumnType(Config.Boolean);

            builder.Property(e => e.ActionTimestamp)
                .HasColumnName("action_timestamp")
                .HasColumnType(Config.DateTime);

            // Foreign key relationship
            builder.HasOne(e => e.BreakGlassAccess)
                .WithMany(a => a.AuditTrail)
                .HasForeignKey(e => e.BreakGlassAccessId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for efficient querying
            builder.HasIndex(e => e.BreakGlassAccessId)
                .HasDatabaseName("IX_BreakGlassAudit_AccessId");

            builder.HasIndex(e => e.ActionTimestamp)
                .HasDatabaseName("IX_BreakGlassAudit_ActionTimestamp");

            builder.HasIndex(e => e.PhiAccessed)
                .HasDatabaseName("IX_BreakGlassAudit_PhiAccessed");
        }
    }
}