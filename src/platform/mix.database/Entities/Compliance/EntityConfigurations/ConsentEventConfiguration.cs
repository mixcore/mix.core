using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Compliance.EntityConfigurations
{
    public class ConsentEventConfiguration : TenantEntityBaseConfiguration<ConsentEvent, int>
    {
        public ConsentEventConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<ConsentEvent> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("mix_consent_event");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnName("user_id")
                .HasColumnType(Config.Guid);

            builder.Property(e => e.PurposeId)
                .HasColumnName("purpose_id")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.Granted)
                .HasColumnName("granted")
                .HasColumnType(Config.Boolean);

            builder.Property(e => e.Method)
                .IsRequired()
                .HasColumnName("method")
                .HasColumnType($"{Config.String}(50)");

            builder.Property(e => e.Version)
                .HasColumnName("version")
                .HasColumnType($"{Config.String}(20)");

            builder.Property(e => e.IpAddress)
                .HasColumnName("ip_address")
                .HasColumnType($"{Config.String}(45)");

            builder.Property(e => e.UserAgent)
                .HasColumnName("user_agent")
                .HasColumnType($"{Config.String}(500)");

            builder.Property(e => e.ConsentTimestamp)
                .HasColumnName("consent_timestamp")
                .HasColumnType(Config.DateTime);

            // Foreign key relationship
            builder.HasOne(e => e.Purpose)
                .WithMany(p => p.ConsentEvents)
                .HasForeignKey(e => e.PurposeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for efficient querying by user and purpose
            builder.HasIndex(e => new { e.TenantId, e.UserId, e.PurposeId })
                .HasDatabaseName("IX_ConsentEvent_Tenant_User_Purpose");

            // Index for consent timestamp for retention queries
            builder.HasIndex(e => e.ConsentTimestamp)
                .HasDatabaseName("IX_ConsentEvent_ConsentTimestamp");
        }
    }
}