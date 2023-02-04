using Mix.Constant.Enums;
using Mix.Heart.Entities;

namespace Mix.Mixdb.Entities
{
    public class MixMetadataContentAssociation : EntityBase<int>
    {
        public MixContentType? ContentType { get; set; }
        public int ContentId { get; set; }
        public int MetadataId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int MixTenantId { get; set; }
    }
}
