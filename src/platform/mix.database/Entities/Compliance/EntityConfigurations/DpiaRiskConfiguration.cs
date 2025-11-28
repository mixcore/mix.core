using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Compliance.EntityConfigurations
{
    public class DpiaRiskConfiguration : TenantEntityBaseConfiguration<DpiaRisk, int>
    {
        public DpiaRiskConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<DpiaRisk> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("mix_dpia_risk");

            builder.Property(e => e.DpiaId)
                .IsRequired()
                .HasColumnName("dpia_id")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.RiskDescription)
                .IsRequired()
                .HasColumnName("risk_description")
                .HasColumnType($"{Config.String}(200)");

            builder.Property(e => e.Category)
                .IsRequired()
                .HasColumnName("category")
                .HasConversion<int>() // Convert enum to int
                .HasColumnType(Config.Integer);

            builder.Property(e => e.Likelihood)
                .HasColumnName("likelihood")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.Impact)
                .HasColumnName("impact")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.ExistingControls)
                .HasColumnName("existing_controls")
                .HasColumnType($"{Config.String}(1000)");

            builder.Property(e => e.ResidualLikelihood)
                .HasColumnName("residual_likelihood")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.ResidualImpact)
                .HasColumnName("residual_impact")
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

            builder.Property(e => e.IdentifiedAt)
                .HasColumnName("identified_at")
                .HasColumnType(Config.DateTime);

            // Foreign key relationship
            builder.HasOne(e => e.Dpia)
                .WithMany(d => d.Risks)
                .HasForeignKey(e => e.DpiaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Navigation property for mitigations
            builder.HasMany(e => e.Mitigations)
                .WithOne(m => m.Risk)
                .HasForeignKey(m => m.RiskId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for efficient querying
            builder.HasIndex(e => e.DpiaId)
                .HasDatabaseName("IX_DpiaRisk_DpiaId");

            builder.HasIndex(e => e.Category)
                .HasDatabaseName("IX_DpiaRisk_Category");

            builder.HasIndex(e => e.Status)
                .HasDatabaseName("IX_DpiaRisk_Status");
        }
    }
}