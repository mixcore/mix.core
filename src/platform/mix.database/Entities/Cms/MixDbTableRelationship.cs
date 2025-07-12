namespace Mix.Database.Entities.Cms
{
    public class MixDbTableRelationship : EntityBase<int>
    {
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public string DisplayName { get; set; }
        public string PropertyName { get; set; }
        public string SourceTableName { get; set; }
        public string DestinateTableName { get; set; }
        public string SourceColumnName { get; set; }
        public string? DestinateColumnName { get; set; }
        public MixDbTableRelationshipType Type { get; set; }
        public virtual MixDbTable SourceDatabase { get; set; }
        public virtual MixDbTable DestinateDatabase { get; set; }
    }
}
