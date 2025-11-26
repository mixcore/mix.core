using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class MixUserTenantConfiguration : AccountEntityBaseConfiguration<MixUserTenant>

    {
        public MixUserTenantConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixUserTenant> builder)
        {
            builder.ToTable("mix_user_tenant");

            builder.HasKey(e => new { e.MixUserId, e.TenantId });

            builder.HasIndex(e => e.MixUserId);

            builder.HasIndex(e => e.TenantId);


            builder.Property(e => e.TenantId)
                 .HasColumnName("tenant_id")
                 .HasColumnType(Config.Integer);
            
            builder.Property(e => e.MixUserId)
                 .HasColumnName("mix_user_id")
                 .HasColumnType(Config.Guid);
        }
    }
}
