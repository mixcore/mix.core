using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class ContactAddressConfiguration : EntityBaseConfiguration<MixContactAddress, int>
    {
        public ContactAddressConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixContactAddress> builder)
        {
            builder.ToTable(MixDbDatabaseNames.ContactAddress);
            base.Configure(builder);

            builder.Property(e => e.Street)
                .IsRequired(false);
            builder.Property(e => e.City)
                .IsRequired(false);
            builder.Property(e => e.Province)
                .IsRequired(false);
            builder.Property(e => e.District)
                .IsRequired(false);
            builder.Property(e => e.Ward).IsRequired(false);
        }
    }
}
