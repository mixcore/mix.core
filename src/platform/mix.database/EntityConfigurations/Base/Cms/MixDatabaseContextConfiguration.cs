using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixDatabaseContextConfiguration<TConfig> : TenantEntityUniqueNameBaseConfiguration<MixDatabaseContext, int, TConfig>
       where TConfig : IDatabaseConstants
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
