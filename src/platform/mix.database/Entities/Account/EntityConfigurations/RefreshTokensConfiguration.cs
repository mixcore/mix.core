using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class RefreshTokensConfiguration : AccountEntityBaseConfiguration<RefreshTokens>

    {
        public RefreshTokensConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<RefreshTokens> builder)
        {
            builder.ToTable("refresh_tokens");
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.ClientId)
                .HasColumnName("client_id")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ExpiresUtc)
                .HasColumnName("expired_utc")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.IssuedUtc)
                .HasColumnName("issued_utc")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.UserName)
                .HasColumnName("user_name")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
        }
    }
}
