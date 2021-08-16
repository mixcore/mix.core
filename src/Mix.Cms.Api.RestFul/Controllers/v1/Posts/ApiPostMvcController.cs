// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.ViewModels.MixPosts;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Heart.Extensions;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/mix-post/mvc")]
    public class ApiPostMvcController :
        BaseReadOnlyApiController<MixCmsContext, MixPost, ReadMvcViewModel>
    {
        public ApiPostMvcController(DefaultRepository<MixCmsContext, MixPost, ReadMvcViewModel> repo) 
            : base(repo)
        {
        }

        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadMvcViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            string type = Request.Query["type"];
            string keyword = Request.Query[MixRequestQueryKeywords.Keyword];
            Expression<Func<MixPost, bool>> predicate = model => model.Specificulture == _lang;
            predicate = predicate.AndAlsoIf(isStatus, model => model.Status == status);
            predicate = predicate.AndAlsoIf(isFromDate, model => model.CreatedDateTime >= fromDate);
            predicate = predicate.AndAlsoIf(isToDate, model => model.CreatedDateTime <= toDate);
            predicate = predicate.AndAlsoIf(!string.IsNullOrEmpty(type), model => model.Type == type);
            predicate = predicate.AndAlsoIf(!string.IsNullOrEmpty(keyword), model => (EF.Functions.Like(model.Title, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Excerpt, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Content, $"%{keyword}%")));
            var getData = await base.GetListAsync(predicate);
            if (getData.IsSucceed)
            {
                return getData.Data;
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        [HttpGet("get-by-attribute")]
        public async Task<ActionResult<PaginationModel<ReadMvcViewModel>>> GetByAttribute()
        {
            var pagingData = new PagingRequest(Request);
            var result = await Helper.GetModelistByMeta<ReadMvcViewModel>(
                Request.Query[MixRequestQueryKeywords.DatabaseName], Request.Query["value"], MixDatabaseNames.ADDITIONAL_COLUMN_POST, pagingData, _lang);
            if (result.IsSucceed)
            {
                return result.Data;
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("get-by-value-id")]
        public async Task<ActionResult<PaginationModel<ReadMvcViewModel>>> GetByValueId()
        {
            var result = await Helper.GetPostListByValueId<ReadMvcViewModel>(
                Request.Query["value"]);
            if (result.IsSucceed)
            {
                return result.Data;
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("get-by-value-ids")]
        public async Task<ActionResult<PaginationModel<ReadMvcViewModel>>> GetByValueIds([FromBody] List<string> valueIds)
        {
            var result = await Mix.Cms.Lib.ViewModels.MixPosts.Helper.GetPostListByValueIds<ReadMvcViewModel>(
                valueIds);
            if (result.IsSucceed)
            {
                return result.Data;
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("search-post")]
        public async Task<ActionResult<PaginationModel<ReadListItemViewModel>>> SearchPost([FromBody] List<string> dataIds, [FromBody] List<string> nestedIds, [FromQuery] string keyword)
        {
            var result = await Mix.Cms.Lib.ViewModels.MixPosts.Helper.SearchPostByIds<ReadListItemViewModel>(
                keyword, dataIds, nestedIds);
            if (result.IsSucceed)
            {
                return result.Data;
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("get-by-data-id")]
        public async Task<ActionResult<PaginationModel<ReadMvcViewModel>>> GetByAttributeDataId()
        {
            var result = await Mix.Cms.Lib.ViewModels.MixPosts.Helper.GetPostListByDataId<ReadMvcViewModel>(
                Request.Query["value"]
                , _lang);
            if (result.IsSucceed)
            {
                return result.Data;
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}