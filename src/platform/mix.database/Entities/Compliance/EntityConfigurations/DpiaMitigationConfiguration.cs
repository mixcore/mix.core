using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Compliance.EntityConfigurations
{
    public class DpiaMitigationConfiguration : EntityBaseConfiguration<DpiaMitigation, int>
    {
        public DpiaMitigationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<DpiaMitigation> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("mix_dpia_mitigation");

            builder.Property(e => e.RiskId)
                .IsRequired()
                .HasColumnName("risk_id")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.MitigationDescription)
                .IsRequired()
                .HasColumnName("mitigation_description")
                .HasColumnType($"{Config.String}(1000)");

            builder.Property(e => e.MitigationType)
                .IsRequired()
                .HasColumnName("mitigation_type")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.ImplementationStatus)
                .IsRequired()
                .HasColumnName("implementation_status")
                .HasColumnType($"{Config.String}(50)");

            builder.Property(e => e.ResponsiblePerson)
                .HasColumnName("responsible_person")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.TargetDate)
                .HasColumnName("target_date")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.ImplementedAt)
                .HasColumnName("implemented_at")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.EffectivenessScore)
                .HasColumnName("effectiveness_score")
                .HasColumnType(Config.Integer);

            // Foreign key relationship
            builder.HasOne(e => e.Risk)
                .WithMany(r => r.Mitigations)
                .HasForeignKey(e => e.RiskId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for efficient querying
            builder.HasIndex(e => e.RiskId)
                .HasDatabaseName("IX_DpiaMitigation_RiskId");

            builder.HasIndex(e => e.ImplementationStatus)
                .HasDatabaseName("IX_DpiaMitigation_ImplementationStatus");

            builder.HasIndex(e => e.TargetDate)
                .HasDatabaseName("IX_DpiaMitigation_TargetDate");
        }
    }
}