namespace Mix.Database.Entities.Base
{
    public abstract class AssociationBase<TPrimaryKey> : EntityBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public int MixTenantId { get; set; }
        public TPrimaryKey ParentId { get; set; }
        public TPrimaryKey ChildId { get; set; }
    }
}
