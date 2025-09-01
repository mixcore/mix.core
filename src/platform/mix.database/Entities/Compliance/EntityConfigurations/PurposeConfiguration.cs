using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Compliance.EntityConfigurations
{
    public class PurposeConfiguration : TenantEntityBaseConfiguration<Purpose, int>
    {
        public PurposeConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<Purpose> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("mix_purpose");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType($"{Config.String}(100)");

            builder.Property(e => e.LawfulBasis)
                .IsRequired()
                .HasColumnName("lawful_basis")
                .HasConversion<int>() // Convert enum to int
                .HasColumnType(Config.Integer);

            builder.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasColumnType(Config.Boolean)
                .HasDefaultValue(true);

            // Navigation properties
            builder.HasMany(e => e.DataFields)
                .WithOne(d => d.Purpose)
                .HasForeignKey(d => d.PurposeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.ConsentEvents)
                .WithOne(c => c.Purpose)
                .HasForeignKey(c => c.PurposeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint for name per tenant
            builder.HasIndex(e => new { e.TenantId, e.Name })
                .IsUnique()
                .HasDatabaseName("IX_Purpose_Tenant_Name");
        }
    }
}