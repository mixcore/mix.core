using Mix.Constant.Enums;

namespace Mix.Services.Databases.Lib.Dtos
{
    public class CreateMetadataContentAssociationDto
    {
        public CreateMetadataContentAssociationDto()
        {

        }
        public int ContentId { get; set; }
        public MixContentType  ContentType { get; set; }
        public int MetadataId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
