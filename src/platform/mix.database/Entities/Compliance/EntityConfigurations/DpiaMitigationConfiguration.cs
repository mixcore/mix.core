using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Compliance.EntityConfigurations
{
    public class DpiaMitigationConfiguration : TenantEntityBaseConfiguration<DpiaMitigation, int>
    {
        public DpiaMitigationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<DpiaMitigation> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("mix_dpia_mitigation");

            builder.Property(e => e.DpiaId)
                .HasColumnName("dpia_id")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.RiskId)
                .IsRequired()
                .HasColumnName("risk_id")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.MitigationDescription)
                .IsRequired()
                .HasColumnName("mitigation_description")
                .HasColumnType($"{Config.String}(200)");

            builder.Property(e => e.Type)
                .IsRequired()
                .HasColumnName("type")
                .HasConversion<int>() // Convert enum to int
                .HasColumnType(Config.Integer);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasConversion<int>() // Convert enum to int
                .HasColumnType(Config.Integer);

            builder.Property(e => e.Owner)
                .HasColumnName("owner")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.TargetDate)
                .HasColumnName("target_date")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.CompletionDate)
                .HasColumnName("completion_date")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.ImplementedAt)
                .HasColumnName("implemented_at")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.ImplementationNotes)
                .HasColumnName("implementation_notes")
                .HasColumnType($"{Config.String}(1000)");

            builder.Property(e => e.EstimatedCost)
                .HasColumnName("estimated_cost")
                .HasColumnType(Config.Decimal);

            builder.Property(e => e.EffectivenessRating)
                .HasColumnName("effectiveness_rating")
                .HasColumnType(Config.Integer);

            // Foreign key relationships
            builder.HasOne(e => e.Dpia)
                .WithMany()
                .HasForeignKey(e => e.DpiaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Risk)
                .WithMany(r => r.Mitigations)
                .HasForeignKey(e => e.RiskId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for efficient querying
            builder.HasIndex(e => e.RiskId)
                .HasDatabaseName("IX_DpiaMitigation_RiskId");

            builder.HasIndex(e => e.Status)
                .HasDatabaseName("IX_DpiaMitigation_Status");

            builder.HasIndex(e => new { e.TenantId, e.Status })
                .HasDatabaseName("IX_DpiaMitigation_Tenant_Status");
        }
    }
}