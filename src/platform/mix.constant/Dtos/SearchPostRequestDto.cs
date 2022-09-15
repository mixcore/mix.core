using Microsoft.AspNetCore.Http;
using Mix.Constant.Constants;
using Mix.Constant.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Constant.Dtos
{
    public class SearchPostRequestDto : SearchRequestDto
    {
        public SearchPostRequestDto()
        {

        }
        public SearchPostRequestDto(SearchRequestDto req)
        {
            Culture = req.Culture;
            Keyword = req.Keyword;
            FromDate = req.FromDate;
            ToDate = req.ToDate;
            PageIndex = req.PageIndex;
            PageSize = req.PageSize;
            OrderBy = req.OrderBy;
            Direction = req.Direction;
            Status = req.Status;
        }

        public string? MixDatabaseName { get; set; }
        public List<SearchQueryField>? Queries { get; set; } = new();
    }
}
