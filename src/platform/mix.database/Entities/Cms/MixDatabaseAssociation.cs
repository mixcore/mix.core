using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms
{
    public class MixDatabaseAssociation : EntityBase<Guid>
    {
        public int MixTenantId { get; set; }
        public string ParentDatabaseName { get; set; }
        public string ChildDatabaseName { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
    }
}
