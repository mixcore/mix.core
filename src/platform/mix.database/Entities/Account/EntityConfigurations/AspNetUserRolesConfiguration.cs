using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class AspNetUserRolesConfiguration : AccountEntityBaseConfiguration<AspNetUserRoles>

    {
        public AspNetUserRolesConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<AspNetUserRoles> builder)
        {
            builder.ToTable("asp_net_user_roles");
            builder.HasKey(e => new { e.UserId, e.RoleId, e.TenantId });

            builder.HasIndex(e => e.RoleId);

            builder.Property(e => e.UserId)
                .HasColumnName("user_id")
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.RoleId)
                .HasColumnName("role_id")
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.TenantId)
                .HasColumnName("tenant_id")
                .HasColumnType(Config.Integer);

        }
    }
}
