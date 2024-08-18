using Mix.Heart.Entities;

namespace Mix.Database.Entities.Account
{
    public class SysMixDatabaseAssociation : EntityBase<int>
    {
        public int MixTenantId { get; set; }
        public string ParentDatabaseName { get; set; }
        public string ChildDatabaseName { get; set; }
        public Guid? GuidParentId { get; set; }
        public Guid? GuidChildId { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
    }
}
