using Mix.Database.Entities.Account;
using Mix.Database.Services;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class MixRolesConfiguration : AccountEntityBaseConfiguration<MixRole>
         
    {
        public MixRolesConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixRole> builder)
        {

            // Error: column "normalizedname" does not exist
            //builder.HasIndex(e => e.NormalizedName)
            //        .HasDatabaseName("MixRoleNameIndex")
            //        .HasFilter("(NormalizedName IS NOT NULL)");

            builder.Property(e => e.Id)
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.ConcurrencyStamp)
            .HasColumnType($"{Config.String}{Config.MediumLength}")
            .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Name)
                .HasColumnType($"{Config.String}{Config.MediumLength}")
            .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.NormalizedName)
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }
    }
}
