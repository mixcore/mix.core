using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class AspNetUserLoginsConfiguration : AccountEntityBaseConfiguration<AspNetUserLogins>

    {
        public AspNetUserLoginsConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<AspNetUserLogins> builder)
        {
            builder.ToTable("asp_net_user_login");
            builder.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins_1");

            builder.HasIndex(e => e.UserId);

            builder.Property(e => e.LoginProvider)
                .HasColumnName("login_provider")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.ProviderKey)
                .HasColumnName("provider_key")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.ProviderDisplayName)
                .HasColumnName("provider_display_name")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnName("user_id")
                .HasDefaultValueSql(Config.GenerateUUID);

        }
    }
}
