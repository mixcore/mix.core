// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.ViewModels.MixPosts;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var searchPostData = new SearchPostQueryModel(Request, _lang);
            var getData = await Helper.SearchPosts<ReadMvcViewModel>(searchPostData);
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