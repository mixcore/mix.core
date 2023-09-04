using Mix.Heart.Enums;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Shared.Dtos;

namespace Mix.Services.Ecommerce.Lib.Dtos
{
    public class FilterOrderDto : SearchRequestDto
    {
        public FilterOrderDto()
        {
            OrderBy = "LastModified";
            Direction = SortDirection.Desc;
        }

        public FilterOrderDto(SearchRequestDto req)
        {
            Culture = req.Culture;
            Keyword = req.Keyword;
            FromDate = req.FromDate;
            ToDate = req.ToDate;
            PageIndex = req.PageIndex;
            PageSize = req.PageSize;
            Status = req.Status;
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderBy = "LastModified";
                Direction = SortDirection.Desc;
            }
        }
        public List<OrderStatus> Statuses { get; set; } = new();
    }
}
