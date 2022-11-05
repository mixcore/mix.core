using Mix.Database.Services;

namespace Mix.Database.Base.Cms
{
    public abstract class AssociationBaseConfiguration<T, TPrimaryKey> : EntityBaseConfiguration<T, TPrimaryKey>
        where TPrimaryKey : IComparable
        where T : AssociationBase<TPrimaryKey>
        
    {
        protected AssociationBaseConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
        }

    }
}
