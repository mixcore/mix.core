using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixDatabaseAssociationConfiguration : EntityBaseConfiguration<MixDatabaseAssociation, Guid>

    {
        public MixDatabaseAssociationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixDatabaseAssociation> builder)
        {
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
