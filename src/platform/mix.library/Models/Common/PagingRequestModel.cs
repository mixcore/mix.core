using Microsoft.AspNetCore.Http;
using Mix.Shared.Constants;
using System;
using Mix.Heart.Extensions;
using Mix.Shared.Services;
using Mix.Shared.Enums;
using Mix.Heart.Enums;

namespace Mix.Lib.Models.Common
{
    public class PagingRequestModel
    {
        public string OrderBy { get; set; }
        public SortDirection Direction { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }

        public PagingRequestModel()
        {

        }
        public PagingRequestModel(HttpRequest request)
        {
            OrderBy = request.Query.TryGetValue(MixRequestQueryKeywords.OrderBy, out var orderBy)
                ? orderBy.ToString().ToTitleCase() : "CreatedDateTime";
            Direction = request.Query.TryGetValue(MixRequestQueryKeywords.Direction, out var direction)
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

            PageSize = request.Query.TryGetValue(MixRequestQueryKeywords.PageSize, out var pageSize)
                ? int.Parse(pageSize) : MixAppSettingService.Instance.GetConfig<int>(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.MaxPageSize);
        }
    }
}
