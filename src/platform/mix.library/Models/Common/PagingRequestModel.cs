using Microsoft.AspNetCore.Http;

namespace Mix.Lib.Models.Common
{
    public class PagingRequestModel : IPagingModel
    {
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public int TotalPage { get; set; }


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
