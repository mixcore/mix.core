using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class AspNetUserTokensConfiguration : AccountEntityBaseConfiguration<AspNetUserTokens>

    {
        public AspNetUserTokensConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<AspNetUserTokens> builder)
        {
            builder.ToTable("asp_net_user_tokens");
            builder.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            builder.Property(e => e.UserId)
                .HasColumnName("user_id")
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.LoginProvider)
                .HasColumnName("login_provider")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Name)
                .HasColumnName("name")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Value)
                .HasColumnName("value")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MaxLength}");

        }
    }
}
