using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class AspNetUserClaimsConfiguration : AccountEntityBaseConfiguration<AspNetUserClaims>

    {
        public AspNetUserClaimsConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<AspNetUserClaims> builder)
        {
            builder.ToTable("asp_net_user_claims");
            builder.HasIndex(e => e.UserId);

            builder.Property(e => e.Id)
                .HasColumnName("id");

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

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnName("user_id")
                .HasDefaultValueSql(Config.GenerateUUID);
            
            builder.Property(e => e.MixUserId)
                .HasColumnName("mix_user_id")
                .HasDefaultValueSql(Config.GenerateUUID);

        }
    }
}
