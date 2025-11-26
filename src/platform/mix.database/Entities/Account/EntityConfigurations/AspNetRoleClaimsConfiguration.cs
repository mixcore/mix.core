using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class AspNetRoleClaimsConfiguration : AccountEntityBaseConfiguration<AspNetRoleClaims>

    {
        public AspNetRoleClaimsConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<AspNetRoleClaims> builder)
        {
            builder.ToTable("asp_net_role_claims");

            builder.Property(e => e.RoleId)
                .HasColumnName("role_id")
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(e => e.ClaimType)
                .HasColumnName("claim_type")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ClaimValue)
                .HasColumnName("claim_value")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

        }
    }
}
