
using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms
{
    public class MixContributor : EntityBase<int>
    {
        public int MixTenantId { get; set; }
        public Guid UserId { get; set; }
        public bool IsOwner { get; set; }
        public int? IntContentId { get; set; }
        public Guid? GuidContentId { get; set; }
        public MixContentType ContentType { get; set; }
    }
}
