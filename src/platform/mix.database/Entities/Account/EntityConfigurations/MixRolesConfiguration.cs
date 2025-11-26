using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class MixRolesConfiguration : AccountEntityBaseConfiguration<MixRole>

    {
        public MixRolesConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixRole> builder)
        {
            builder.ToTable("mix_roles");
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.ConcurrencyStamp)
                .HasColumnName("concurrency_stamp")
            .HasColumnType($"{Config.String}{Config.MediumLength}")
            .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Name)
                .HasColumnName("name")
                .HasColumnType($"{Config.String}{Config.MediumLength}")
            .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.NormalizedName)
                .HasColumnName("normalized_name")
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }
    }
}
