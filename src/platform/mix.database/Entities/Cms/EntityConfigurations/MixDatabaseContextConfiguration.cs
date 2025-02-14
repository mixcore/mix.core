using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Extensions;

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
                .HasColumnName("schema")
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
            
            builder.Property(e => e.AesKey)
                .IsRequired()
                .HasColumnName("aes_key")
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
            
            builder.Property(e => e.ConnectionString)
                .IsRequired()
                .HasColumnName("connection_string")
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);

            builder.Property(e => e.DatabaseProvider)
               .IsRequired()
               .HasColumnName("database_provider")
               .HasConversion(new EnumToStringConverter<MixDatabaseProvider>())
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);

            builder.Property(e => e.NamingConvention)
              .IsRequired()
              .HasColumnName("naming_convention")
              .HasConversion(new EnumToStringConverter<MixDatabaseNamingConvention>())
              .HasColumnType($"{Config.NString}{Config.SmallLength}")
              .HasCharSet(Config.CharSet);

        }
    }
}
