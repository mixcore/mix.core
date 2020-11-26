// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Controllers;
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
    [Route("api/v1/rest/{culture}/post/mvc")]
    public class ApiPostMvcController :
        BaseRestApiController<MixCmsContext, MixPost, ReadMvcViewModel, ReadListItemViewModel>
    {

        // GET: api/s
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadListItemViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query["status"], out MixEnums.MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            string type = Request.Query["type"];
            string keyword = Request.Query["keyword"];
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

        [HttpGet("get-by-attribute")]
        public async Task<ActionResult<PaginationModel<ReadMvcViewModel>>> GetByAttribute()
        {
            var result = await Mix.Cms.Lib.ViewModels.MixPosts.Helper.GetModelistByMeta<ReadMvcViewModel>(
                Request.Query["attributeSetName"], Request.Query["value"], _lang);
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
            var result = await Mix.Cms.Lib.ViewModels.MixPosts.Helper.GetPostListByValueId<ReadMvcViewModel>(
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
        public async Task<ActionResult<PaginationModel<ReadListItemViewModel>>> SearchPost([FromBody] List<string> dataIds, [FromQuery] string keyword)
        {
            var result = await Mix.Cms.Lib.ViewModels.MixPosts.Helper.SearchPost<ReadListItemViewModel>(
                keyword, dataIds);
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