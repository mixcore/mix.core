namespace Mix.Database.Entities.Base
{
    public abstract class TenantEntityUniqueNameBase<TPrimaryKey> : TenantEntityBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public string SystemName { get; set; }
    }
}
