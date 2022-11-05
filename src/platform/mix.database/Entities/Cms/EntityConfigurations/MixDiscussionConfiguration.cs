using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixDiscussionConfiguration : EntityBaseConfiguration<MixDiscussion, int>
        
    {
        public MixDiscussionConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

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
