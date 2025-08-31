using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Compliance.EntityConfigurations
{
    public class DpiaRiskConfiguration : EntityBaseConfiguration<DpiaRisk, int>
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
                .HasColumnType($"{Config.String}(1000)");

            builder.Property(e => e.Category)
                .IsRequired()
                .HasColumnName("category")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.Likelihood)
                .HasColumnName("likelihood")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.Impact)
                .HasColumnName("impact")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.RiskLevel)
                .IsRequired()
                .HasColumnName("risk_level")
                .HasColumnType($"{Config.String}(20)");

            builder.Property(e => e.IdentifiedAt)
                .HasColumnName("identified_at")
                .HasColumnType(Config.DateTime);

            // Foreign key relationship
            builder.HasOne(e => e.Dpia)
                .WithMany(d => d.Risks)
                .HasForeignKey(e => e.DpiaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for efficient querying
            builder.HasIndex(e => e.DpiaId)
                .HasDatabaseName("IX_DpiaRisk_DpiaId");

            builder.HasIndex(e => e.Category)
                .HasDatabaseName("IX_DpiaRisk_Category");

            builder.HasIndex(e => e.RiskLevel)
                .HasDatabaseName("IX_DpiaRisk_RiskLevel");
        }
    }
}