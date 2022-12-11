using Mix.Services.Ecommerce.Lib.Constants;
using Mix.Shared.Dtos;

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
    }
}
