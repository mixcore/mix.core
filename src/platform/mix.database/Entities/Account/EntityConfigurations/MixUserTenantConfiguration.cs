using Mix.Database.Services;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class MixUserTenantConfiguration : AccountEntityBaseConfiguration<MixUserTenant>

    {
        public MixUserTenantConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixUserTenant> builder)
        {
            builder.HasKey(e => new { e.MixUserId, e.TenantId });

            builder.HasIndex(e => e.MixUserId);

            builder.HasIndex(e => e.TenantId);


            builder.Property(e => e.MixUserId)
                 .HasDefaultValueSql(Config.GenerateUUID);
        }
    }
}
