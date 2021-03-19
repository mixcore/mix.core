using Microsoft.AspNetCore.Http;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Services;
using Mix.Heart.Enums;
using System;

namespace Mix.Cms.Lib.Models.Common
{
    public class PagingDataModel
    {
        public string OrderBy { get; set; }
        public MixHeartEnums.DisplayDirection Direction { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Page { get => PageIndex + 1; }

        public PagingDataModel()
        {

        }
        public PagingDataModel(HttpRequest request)
        {
            OrderBy = request.Query.TryGetValue(MixRequestQueryKeywords.OrderBy, out var orderBy)
                ? orderBy : "CreatedDateTime";
            Direction = request.Query.TryGetValue(MixRequestQueryKeywords.Direction, out var direction)
                ? Enum.Parse<MixHeartEnums.DisplayDirection>(direction) : MixHeartEnums.DisplayDirection.Desc;
            PageIndex = request.Query.TryGetValue(MixRequestQueryKeywords.PageIndex, out var pageIndex)
                ? int.Parse(pageIndex) : 0;
            PageSize = request.Query.TryGetValue(MixRequestQueryKeywords.PageSize, out var pageSize)
                ? int.Parse(pageSize) : MixService.GetConfig<int>(MixAppSettingKeywords.MaxPageSize);
        }
    }
}
