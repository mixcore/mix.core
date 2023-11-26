using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class OAuthTokenConfiguration : EntityBaseConfiguration<OAuthToken, Guid>

    {
        public OAuthTokenConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<OAuthToken> builder)
        {
            builder.Property(e => e.ClientId)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            base.Configure(builder);
        }
    }
}
