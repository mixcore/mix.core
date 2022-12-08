using Mix.Services.Databases.Lib.Enums;

namespace Mix.Services.Databases.Lib.Dtos
{
    public class CreateMetadataDto
    {
        public CreateMetadataDto()
        {

        }
        public string Type { get; set; }
        public string Content { get; set; }
        public string? SeoContent { get; set; }
    }
}
