using Microsoft.AspNetCore.Http;
using Mix.Constant.Constants;
using Mix.Constant.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.RepoDb.Dtos
{
    public class SearchMixDbRequestDto : SearchRequestDto
    {
        public SearchMixDbRequestDto()
        {

        }
        public SearchMixDbRequestDto(SearchRequestDto req, HttpRequest request)
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

            if (int.TryParse(request.Query[MixRequestQueryKeywords.ParentId], out int parentId))
            {
                ParentId = parentId;
            }
        }

        public int? ParentId { get; set; }
        public string? ParentName { get; set; }
    }
}
