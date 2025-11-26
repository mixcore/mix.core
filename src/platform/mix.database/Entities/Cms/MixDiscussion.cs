namespace Mix.Database.Entities.Cms
{
    public class MixDiscussion : EntityBase<int>
    {
        public int TenantId { get; set; }
        public int? ParentId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public int? IntContentId { get; set; }
        public Guid? GuidContentId { get; set; }
        public MixContentType ContentType { get; set; }
    }
}
