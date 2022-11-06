using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixTenantConfiguration : EntityBaseConfiguration<MixTenant, int>

    {
        public MixTenantConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixTenant> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.PrimaryDomain)
                .IsRequired()
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Description)
                .HasColumnType($"{Config.NString}{Config.MaxLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }
    }
}
