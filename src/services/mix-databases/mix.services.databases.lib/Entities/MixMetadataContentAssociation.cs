using Mix.Heart.Entities;
using Mix.Services.Databases.Lib.Enums;

namespace Mix.Services.Databases.Lib.Entities
{
    public class MixMetadataContentAssociation : EntityBase<int>
    {
        public MetadataParentType? ContentType { get; set; }
        public int ContentId { get; set; }
        public int MetadataId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int MixTenantId { get; set; }
    }
}
