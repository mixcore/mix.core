using Azure.Core;
using Microsoft.AspNetCore.Http;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Mix.Heart.Model;
using Mix.Heart.Models;
using Mix.Shared.Dtos;
using System.Globalization;
using System.Net.NetworkInformation;

namespace Mix.Shared.Models
{
    public class PagingRequestModel : PagingModel
    {
        protected readonly int DefaultPageSize;

        public PagingRequestModel(int defaultPageSize = MixConstants.CONST_DEFAULT_PAGESIZE)
        {
            DefaultPageSize = defaultPageSize;
        }

        public PagingRequestModel(SearchRequestDto request)
        {
            Page = request.PageIndex + 1;
            PagingState = request.PagingState;
            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
            SortByColumns = request.SortByFields;
            if (SortByColumns == null && !string.IsNullOrEmpty(request.SortBy))
            {
                SortByColumns = new List<MixSortByColumn>()
                {
                    new MixSortByColumn(request.SortBy, request.Direction)
                };
            }
        }

        public PagingRequestModel(SearchMixDbRequestDto request)
        {
            Page = request.PageIndex + 1;
            PagingState = request.PagingState;
            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
            SortByColumns = request.SortByColumns ?? new List<MixSortByColumn>();
            if (request.SortByColumns == null)
            {
                if (!string.IsNullOrEmpty(request.SortBy))
                {
                    SortByColumns.Add(new Heart.Model.MixSortByColumn(request.SortBy, request.Direction));
                }
            }
        }

        public PagingRequestModel(HttpRequest request, int defaultPageSize = MixConstants.CONST_DEFAULT_PAGESIZE)
        {
            DefaultPageSize = defaultPageSize;
            request.Query.TryGetValue(MixRequestQueryKeywords.PageSize, out var strPageSize);
            int.TryParse(strPageSize, out int pageSize);


            if (request.Query.TryGetValue(MixRequestQueryKeywords.OrderBy, out var orderBy))
            {
                var sortDirection = SortDirection.Asc;
                if (request.Query.TryGetValue(MixRequestQueryKeywords.Direction, out var direction))
                {
                    sortDirection = Enum.Parse<SortDirection>(direction);
                }
                SortByColumns = new List<MixSortByColumn>()
                {
                    new MixSortByColumn(orderBy, sortDirection)
                };
            }
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
