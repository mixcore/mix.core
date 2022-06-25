using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.SQLSERVER.Base;


namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class MixDatabaseContextConfiguration : SqlServerTenantEntityUniqueNameBaseConfiguration<MixDatabaseContext, int>
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
