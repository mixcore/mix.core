using Mix.Database.Services;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class AspNetUserRolesConfiguration : AccountEntityBaseConfiguration<AspNetUserRoles>

    {
        public AspNetUserRolesConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<AspNetUserRoles> builder)
        {
            builder.HasKey(e => new { e.UserId, e.RoleId, e.MixTenantId });

            builder.HasIndex(e => e.RoleId);

            builder.Property(e => e.UserId)
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.RoleId)
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.MixTenantId)
                .HasColumnType(Config.Integer);

        }
    }
}
