using Microsoft.AspNetCore.Http;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Mix.Heart.Models;


namespace Mix.Shared.Models
{
    public class PagingRequestModel : PagingModel
    {
        public int Page { get; set; }
        protected readonly int _defaultPageSize;

        public PagingRequestModel(int defaultPageSize = 1000)
        {
            _defaultPageSize = defaultPageSize;
        }

        public PagingRequestModel(HttpRequest request, int defaultPageSize = 1000)
        {
            _defaultPageSize = defaultPageSize;
            request.Query.TryGetValue(MixRequestQueryKeywords.PageSize, out var strPageSize);
            int.TryParse(strPageSize, out int pageSize);


            SortBy = request.Query.TryGetValue(MixRequestQueryKeywords.OrderBy, out var orderBy)
                ? orderBy.ToString().ToTitleCase() : "CreatedDateTime";
            SortDirection = request.Query.TryGetValue(MixRequestQueryKeywords.Direction, out var direction)
                ? Enum.Parse<SortDirection>(direction) : SortDirection.Desc;
            Page = request.Query.TryGetValue(MixRequestQueryKeywords.Page, out var page)
                ? int.Parse(page) : 0;

            if (Page > 0)
            {
                PageIndex = Page - 1;
            }
            else
            {
                PageIndex = request.Query.TryGetValue(MixRequestQueryKeywords.PageIndex, out var pageIndex)
                ? int.Parse(pageIndex) : 0;
                Page = PageIndex + 1;
            }

            PageSize = pageSize > 0
                        ? pageSize
                        : defaultPageSize;
        }
    }
}
