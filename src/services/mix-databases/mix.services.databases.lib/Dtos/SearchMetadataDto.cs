using Mix.Constant.Enums;
using Mix.Shared.Dtos;

namespace Mix.Services.Databases.Lib.Dtos
{
    public class SearchMetadataDto : SearchRequestDto
    {
        public SearchMetadataDto()
        {
        }

        #region Properties

        public int ContentId { get; set; }
        public MixContentType? ContentType { get; set; }
        public string MetadataType { get; set; }

        #endregion
    }
}
