using Microsoft.AspNetCore.Http;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Services;
using Mix.Heart.Enums;
using System;
using Mix.Heart.Extensions;

namespace Mix.Cms.Lib.Models.Common
{
    public class PagingRequest
    {
        public string OrderBy { get; set; }
        public DisplayDirection Direction { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }

        public PagingRequest()
        {

        }
        public PagingRequest(HttpRequest request)
        {
            OrderBy = request.Query.TryGetValue(MixRequestQueryKeywords.OrderBy, out var orderBy)
                ? orderBy.ToString().ToTitleCase() : "CreatedDateTime";
            Direction = request.Query.TryGetValue(MixRequestQueryKeywords.Direction, out var direction)
                ? Enum.Parse<DisplayDirection>(direction) : DisplayDirection.Desc;
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

            PageSize = request.Query.TryGetValue(MixRequestQueryKeywords.PageSize, out var pageSize)
                ? int.Parse(pageSize) : MixService.GetAppSetting<int>(MixAppSettingKeywords.MaxPageSize);
        }
    }
}
