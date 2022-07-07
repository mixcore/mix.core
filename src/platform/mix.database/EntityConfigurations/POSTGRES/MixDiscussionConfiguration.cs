using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.POSTGRES.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class MixDiscussionConfiguration : PostgresEntityBaseConfiguration<MixDiscussion, int>
    {
        public override void Configure(EntityTypeBuilder<MixDiscussion> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Content)
             .IsRequired()
             .HasColumnType(Config.Text)
             .HasCharSet(Config.CharSet);

            builder.Property(e => e.ContentType)
             .IsRequired()
             .HasConversion(new EnumToStringConverter<MixContentType>())
             .HasColumnType($"{Config.NString}{Config.SmallLength}")
             .HasCharSet(Config.CharSet);
        }
    }
}
