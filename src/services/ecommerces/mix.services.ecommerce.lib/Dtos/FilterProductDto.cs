using Mix.Services.Ecommerce.Lib.Constants;
using Mix.Services.Ecommerce.Lib.Entities;
using Mix.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Ecommerce.Lib.Dtos
{
    public class FilterProductDto : SearchRequestDto
    {
        public FilterProductDto()
        {

        }

        public FilterProductDto(SearchRequestDto req)
        {
            Culture = req.Culture;
            Keyword = req.Keyword;
            FromDate = req.FromDate;
            ToDate = req.ToDate;
            PageIndex = req.PageIndex;
            PageSize = req.PageSize;
            OrderBy = req.OrderBy;
            Direction = req.Direction;
            Status = req.Status;
        }

        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string MixDatabaseName { get => MixEcommerceConstants.DatabaseNameProductDetails; }
        public ProductMetadataDto Metadata { get; set; } = new();
        public bool IsFilterMetadata => Metadata != null && (Metadata.Interior != null || Metadata.Tile != null || Metadata.Brands != null || Metadata.Decor != null);
    }

    public class ProductMetadataDto
    {
        public string[]? Tile { get; set; }
        public string[]? Interior { get; set; }
        public string[]? Lighting { get; set; }
        public string[]? Decor { get; set; }
        public string[]? Brands { get; set; }
    }
}
