namespace Mix.Database.Entities.Cms
{
    public class MixDbTableRelationship : EntityBase<int>
    {
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public string DisplayName { get; set; }
        public string SourceDatabaseName { get; set; }
        public string DestinateDatabaseName { get; set; }
        public MixDbTableRelationshipType Type { get; set; }
        public virtual MixDbTable SourceDatabase { get; set; }
        public virtual MixDbTable DestinateDatabase { get; set; }
    }
}
