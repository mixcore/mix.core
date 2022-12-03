using Mix.Constant.Enums;
using Mix.Services.Databases.Lib.Enums;

namespace Mix.Services.Databases.Lib.Dtos
{
    public class CreateMetadataLinkDto
    {
        public int ParentId { get; set; }
        public MetadataParentType  ParentType { get; set; }
        public int MetadataId { get; set; }
        public string Description { get; set; }
    }
}
