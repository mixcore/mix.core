namespace Mix.Database.Entities.Cms
{
    public class MixDbDataAssociation : EntityBase<Guid>
    {
        public int TenantId { get; set; }
        public string ParentDatabaseName { get; set; }
        public string ChildDatabaseName { get; set; }
        public Guid? GuidParentId { get; set; }
        public Guid? GuidChildId { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
    }
}
