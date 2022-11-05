using Mix.Database.Base;
using Mix.Database.EntityConfigurations.POSTGRES.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixPostContentConfiguration : MixPostContentConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixPostContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ClassName)
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}
