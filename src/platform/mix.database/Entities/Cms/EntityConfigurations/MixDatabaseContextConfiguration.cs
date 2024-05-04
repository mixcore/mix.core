using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Base.Cms;
using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixDatabaseContextConfiguration : TenantEntityUniqueNameBaseConfiguration<MixDatabaseContext, int>

    {
        public MixDatabaseContextConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixDatabaseContext> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Schema)
                .IsRequired()
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);

            builder.Property(e => e.ConnectionString)
                .IsRequired()
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);

            builder.Property(e => e.DatabaseProvider)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDatabaseProvider>())
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);

            builder.Property(e => e.NamingConvention)
              .IsRequired()
              .HasConversion(new EnumToStringConverter<MixDatabaseNamingConvention>())
              .HasColumnType($"{Config.NString}{Config.SmallLength}")
              .HasCharSet(Config.CharSet);

        }
    }
}
