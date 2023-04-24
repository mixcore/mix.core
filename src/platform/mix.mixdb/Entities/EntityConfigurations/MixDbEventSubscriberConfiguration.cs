using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;
using Newtonsoft.Json.Linq;

namespace Mix.Mixdb.Entities.EntityConfigurations
{
    public class MixDbEventSubscriberConfiguration : EntityBaseConfiguration<MixDbEventSubscriber, int>
    {
        public MixDbEventSubscriberConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixDbEventSubscriber> builder)
        {
            builder.ToTable(MixDbDatabaseNames.DatabaseNameMixDbEventSubscriber);
            builder.Property(e => e.Callback)
             .HasConversion(
                 v => v.ToString(Newtonsoft.Json.Formatting.None),
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : new())
             .IsRequired(false)
             .HasColumnType(Config.Text);

            base.Configure(builder);
        }
    }
}
