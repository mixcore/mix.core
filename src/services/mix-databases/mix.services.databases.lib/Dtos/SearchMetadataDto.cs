using Mix.Services.Databases.Lib.Enums;
using Mix.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Databases.Lib.Dtos
{
    public class SearchMetadataDto: SearchRequestDto
    {
        public SearchMetadataDto()
        {
        }

        #region Properties

        public int ContentId { get; set; }
        public MetadataParentType? ContentType { get; set; }
        public string MetadataType { get; set; }

        #endregion
    }
}
