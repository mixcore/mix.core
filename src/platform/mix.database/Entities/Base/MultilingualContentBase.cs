namespace Mix.Database.Entities.Base
{
    public abstract class MultilingualContentBase<TPrimaryKey> : EntityBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public int TenantId { get; set; }
        public string Specificulture { get; set; }
        public string Icon { get; set; }
        public bool IsPublic { get; set; }
        public TPrimaryKey ParentId { get; set; }
        public int MixCultureId { get; set; }
        public virtual MixCulture MixCulture { get; set; }
    }
}
