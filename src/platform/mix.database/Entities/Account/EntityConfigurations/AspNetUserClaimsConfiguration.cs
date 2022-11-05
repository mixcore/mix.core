using Mix.Database.Entities.Account;
using Mix.Database.Services;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class AspNetUserClaimsConfiguration : AccountEntityBaseConfiguration<AspNetUserClaims>
         
    {
        public AspNetUserClaimsConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<AspNetUserClaims> builder)
        {
            builder.HasIndex(e => e.UserId);

            builder.Property(e => e.Id);

            builder.Property(e => e.ClaimType)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ClaimValue)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.HasOne(d => d.MixUser)
                    .WithMany(p => p.AspNetUserClaimsUser)
                    .HasForeignKey(d => d.UserId);

        }
    }
}
