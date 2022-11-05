using Mix.Database.Entities.Account;
using Mix.Database.Services;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class RefreshTokensConfiguration : AccountEntityBaseConfiguration<RefreshTokens>
         
    {
        public RefreshTokensConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<RefreshTokens> builder)
        {
            builder.Property(e => e.Id)
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.ClientId)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Email)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ExpiresUtc)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.IssuedUtc)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.Username)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
        }
    }
}
