using Mix.Database.EntityConfigurations.Base;
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
                .HasColumnType(Config.Integer);

            builder.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasColumnType(Config.Boolean)
                .HasDefaultValue(true);

            // Unique constraint for name per tenant
            builder.HasIndex(e => new { e.TenantId, e.Name })
                .IsUnique()
                .HasDatabaseName("IX_Purpose_Tenant_Name");
        }
    }
}