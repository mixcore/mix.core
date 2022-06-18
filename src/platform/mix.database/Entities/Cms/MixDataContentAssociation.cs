using Mix.Database.Entities.Base;


namespace Mix.Database.Entities.Cms
{
    public class MixDataContentAssociation : MultilingualContentBase<Guid>
    {
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public Guid DataContentId { get; set; }
        public Guid? GuidParentId { get; set; }
        public int? IntParentId { get; set; }
    }
}
