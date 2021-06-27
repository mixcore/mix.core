using Microsoft.AspNetCore.Http;
using Mix.Shared.Constants;
using Mix.Lib.Dtos;
using Mix.Shared.Enums;
using Mix.Lib.Services;
using System;
using Mix.Shared.Services;
using Mix.Heart.Enums;

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
        
        public SearchQueryModel(HttpRequest request, MixAppSettingService appSettingService)
        {
            string culture = appSettingService.GetConfig<string>(
                MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.DefaultCulture);
            Init(request, culture, appSettingService);
        }
        
        public SearchQueryModel(HttpRequest request, string culture, MixAppSettingService appSettingService)
        {
            Init(request, culture, appSettingService);
        }

        

        public SearchQueryModel(SearchRequestDto request, string culture, MixAppSettingService appSettingService)
        {
            Specificulture = culture ?? appSettingService.GetConfig<string>(
                MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.DefaultCulture);
            FromDate = request.FromDate;
            ToDate = request.ToDate;
            Status = request.Status;
            Keyword = request.Keyword;
            PagingData = new PagingRequestModel() {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SortDirection = request.Direction,
                SortBy = request.OrderBy
            };
        }

        private void Init(HttpRequest request, string culture, MixAppSettingService appSettingService)
        {
            Specificulture = culture;
            FromDate = DateTime.TryParse(request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate)
                ? fromDate : null;
            ToDate = DateTime.TryParse(request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate)
                ? toDate : null;
            Status = Enum.TryParse(request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status)
                ? status : null;
            Keyword = request.Query.TryGetValue(MixRequestQueryKeywords.Keyword, out var orderBy)
                ? orderBy : string.Empty;
            PagingData = new PagingRequestModel(request, appSettingService);
        }
    }
}
