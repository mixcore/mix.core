using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.SQLITE.Base;


namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class MixDatabaseContextConfiguration : SqliteTenantEntityUniqueNameBaseConfiguration<MixDatabaseContext, int>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseContext> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ConnectionString)
                .IsRequired()
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);

            builder.Property(e => e.DatabaseProvider)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDatabaseProvider>())
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}
