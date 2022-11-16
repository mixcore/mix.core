using Mix.Database.Services;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class AspNetUserTokensConfiguration : AccountEntityBaseConfiguration<AspNetUserTokens>

    {
        public AspNetUserTokensConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<AspNetUserTokens> builder)
        {
            builder.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            builder.Property(e => e.UserId)
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.LoginProvider)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Name)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Value)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MaxLength}");

        }
    }
}
