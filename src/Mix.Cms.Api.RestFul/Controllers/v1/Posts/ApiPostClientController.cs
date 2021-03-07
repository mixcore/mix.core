// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixPosts;
using Mix.Domain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/post/client")]
    public class ApiPostClientController :
        BaseReadOnlyApiController<MixCmsContext, MixPost, ReadClientViewModel>
    {
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadClientViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            string type = Request.Query["type"];
            string keyword = Request.Query[MixRequestQueryKeywords.Keyword];
            Expression<Func<MixPost, bool>> predicate = model =>
                model.Specificulture == _lang
                && (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(type) || model.Type == type)
                && (string.IsNullOrEmpty(keyword)
                 || (EF.Functions.Like(model.Title, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Excerpt, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Content, $"%{keyword}%"))
                 );
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

        [HttpGet("get-by-meta")]
        public async Task<ActionResult<PaginationModel<ReadClientViewModel>>> GetByMeta()
        {
            var result = await Helper.GetModelistByMeta<ReadClientViewModel>(
                Request.Query[MixRequestQueryKeywords.DatabaseName], Request.Query["value"], _lang);
            if (result.IsSucceed)
            {
                return result.Data;
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        
        [HttpGet("get-by-value")]
        public async Task<ActionResult<PaginationModel<ReadClientViewModel>>> GetByValue()
        {
            var result = await Helper.SearchPostByField<ReadClientViewModel>(
                Request.Query["column"], Request.Query["value"], _lang);
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
        public async Task<ActionResult<PaginationModel<ReadClientViewModel>>> GetByValueId()
        {
            var result = await Mix.Cms.Lib.ViewModels.MixPosts.Helper.GetPostListByValueId<ReadClientViewModel>(
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
        public async Task<ActionResult<PaginationModel<ReadClientViewModel>>> GetByValueIds([FromBody] List<string> valueIds)
        {
            var result = await Mix.Cms.Lib.ViewModels.MixPosts.Helper.GetPostListByValueIds<ReadClientViewModel>(
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
        public async Task<ActionResult<PaginationModel<ReadClientViewModel>>> GetByAttributeDataId()
        {
            var result = await Mix.Cms.Lib.ViewModels.MixPosts.Helper.GetPostListByDataId<ReadClientViewModel>(
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