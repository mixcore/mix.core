using Mix.Database.Services;

namespace Mix.Database.Base.Cms
{
    public abstract class TenantEntityUniqueNameBaseConfiguration<T, TPrimaryKey> : TenantEntityBaseConfiguration<T, TPrimaryKey>
        where TPrimaryKey : IComparable
        where T : TenantEntityUniqueNameBase<TPrimaryKey>

    {
        protected TenantEntityUniqueNameBaseConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SystemName)
               .IsRequired()
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

        }

    }
}
