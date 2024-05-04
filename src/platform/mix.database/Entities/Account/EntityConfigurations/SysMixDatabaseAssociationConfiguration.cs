using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    public class SysMixDatabaseAssociationConfiguration : EntityBaseConfiguration<SysMixDatabaseAssociation, int>

    {
        public SysMixDatabaseAssociationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<SysMixDatabaseAssociation> builder)
        {
            builder.ToTable(MixDatabaseNames.SYSTEM_DATA_RELATIONSHIP);
            base.Configure(builder);

            builder.Property(e => e.ParentDatabaseName)
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.ChildDatabaseName)
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}
