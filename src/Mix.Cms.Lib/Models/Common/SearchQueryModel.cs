using Microsoft.AspNetCore.Http;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Services;
using System;

namespace Mix.Cms.Lib.Models.Common
{
    public class SearchQueryModel
    {
        public string Specificulture { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public MixContentStatus? Status { get; set; }
        public string Keyword { get; set; }
        public PagingRequest PagingData { get; set; }

        public SearchQueryModel()
        {

        }
        public SearchQueryModel(HttpRequest request, string culture = null)
        {
            Specificulture = culture ?? MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
            FromDate = DateTime.TryParse(request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate)
                ? fromDate: null;
            ToDate = DateTime.TryParse(request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate)
                ? toDate: null;
            Status = Enum.TryParse(request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status)
                ? status : null;
            Keyword = request.Query.TryGetValue(MixRequestQueryKeywords.Keyword, out var orderBy)
                ? orderBy : string.Empty;
            PagingData = new PagingRequest(request);
        }
    }
}
