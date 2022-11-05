using Mix.Database.Entities.Account;
using Mix.Database.Services;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class AspNetRoleClaimsConfiguration : AccountEntityBaseConfiguration<AspNetRoleClaims>
         
    {
        public AspNetRoleClaimsConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<AspNetRoleClaims> builder)
        {
            builder.HasIndex(e => e.RoleId);

            builder.Property(e => e.RoleId)
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.Property(e => e.ClaimType)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ClaimValue)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.RoleId)
                .IsRequired()
                .HasDefaultValueSql(Config.GenerateUUID);

        }
    }
}
