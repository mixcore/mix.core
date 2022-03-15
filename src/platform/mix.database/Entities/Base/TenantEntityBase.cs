using Mix.Database.Entities.Cms;

namespace Mix.Database.Entities.Base
{
    public abstract class TenantEntityBase<TPrimaryKey> : EntityBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public string DisplayName { get; set; }
        public virtual string Description { get; set; }

        public int MixTenantId { get; set; }
        public MixTenant MixTenant { get; set; }
    }
}
