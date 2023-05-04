using Mix.Database.Base.Cms;
using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixDomainConfiguration : TenantEntityBaseConfiguration<MixDomain, int>

    {
        public MixDomainConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixDomain> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Host)
                .IsRequired()
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }
    }
}
