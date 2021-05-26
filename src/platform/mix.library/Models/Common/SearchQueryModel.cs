using Microsoft.AspNetCore.Http;
using Mix.Lib.Constants;
using Mix.Lib.Dtos;
using Mix.Lib.Enums;
using Mix.Lib.Services;
using System;

namespace Mix.Lib.Models.Common
{
    public class SearchQueryModel
    {
        public string Specificulture { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public MixContentStatus? Status { get; set; }
        public string Keyword { get; set; }
        public PagingRequestModel PagingData { get; set; }

        public SearchQueryModel()
        {

        }
        
        public SearchQueryModel(HttpRequest request, string culture = null)
        {
            Specificulture = culture ?? MixService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
            FromDate = DateTime.TryParse(request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate)
                ? fromDate: null;
            ToDate = DateTime.TryParse(request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate)
                ? toDate: null;
            Status = Enum.TryParse(request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status)
                ? status : null;
            Keyword = request.Query.TryGetValue(MixRequestQueryKeywords.Keyword, out var orderBy)
                ? orderBy : string.Empty;
            PagingData = new PagingRequestModel(request);
        }
        
        public SearchQueryModel(SearchRequestDto request, string culture = null)
        {
            Specificulture = culture ?? MixService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
            FromDate = request.FromDate;
            ToDate = request.ToDate;
            Status = request.Status;
            Keyword = request.Keyword;
            PagingData = new PagingRequestModel() {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Direction = request.Direction,
                OrderBy = request.OrderBy
            };
        }
    }
}
