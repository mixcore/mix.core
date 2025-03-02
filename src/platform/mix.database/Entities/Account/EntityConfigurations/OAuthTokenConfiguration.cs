using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class OAuthTokenConfiguration : EntityBaseConfiguration<OAuthToken, Guid>

    {
        public OAuthTokenConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<OAuthToken> builder)
        {
            builder.ToTable("o_auth_token");

            builder.Property(e => e.TokenType)
                .HasColumnName("token_type");
            
            builder.Property(e => e.Token)
                .HasColumnName("token");

            builder.Property(e => e.SubjectId)
                .HasColumnName("subject_id");

            builder.Property(e => e.CreationDate)
                .HasColumnName("creation_date");

            builder.Property(e => e.ExpirationDate)
                .HasColumnName("expiration_date");

            builder.Property(e => e.ReferenceId)
                .HasColumnName("reference_id");

            builder.Property(e => e.TokenTypeHint)
                .HasColumnName("token_type_hint");

            builder.Property(e => e.TokenStatus)
                .HasColumnName("token_status");

            builder.Property(e => e.Revoked)
                .HasColumnName("revoked");

            builder.Property(e => e.ClientId)
                .HasColumnName("client_id")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            base.Configure(builder);
        }
    }
}
