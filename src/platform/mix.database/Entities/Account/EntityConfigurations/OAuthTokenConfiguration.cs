using Mix.Database.Services;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class OAuthTokenConfiguration : AccountEntityBaseConfiguration<OAuthToken>

    {
        public OAuthTokenConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<OAuthToken> builder)
        {
            builder.Property(e => e.Id)
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.ClientId)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");
        }
    }
}
