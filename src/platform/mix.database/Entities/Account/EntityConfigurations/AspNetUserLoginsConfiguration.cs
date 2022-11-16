using Mix.Database.Services;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class AspNetUserLoginsConfiguration : AccountEntityBaseConfiguration<AspNetUserLogins>

    {
        public AspNetUserLoginsConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<AspNetUserLogins> builder)
        {
            builder.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins_1");

            builder.HasIndex(e => e.UserId);

            builder.Property(e => e.LoginProvider)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.ProviderKey)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.ProviderDisplayName)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasDefaultValueSql(Config.GenerateUUID);

        }
    }
}
