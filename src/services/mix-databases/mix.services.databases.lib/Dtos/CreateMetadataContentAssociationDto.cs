using Mix.Services.Databases.Lib.Enums;

namespace Mix.Services.Databases.Lib.Dtos
{
    public class CreateMetadataContentAssociationDto
    {
        public int ContentId { get; set; }
        public MetadataParentType  ContentType { get; set; }
        public int MetadataId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
