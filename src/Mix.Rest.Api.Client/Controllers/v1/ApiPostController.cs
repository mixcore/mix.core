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
using Mix.Rest.Api.Client.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Rest.Api.Client.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/post/client")]
    public class ApiPostClientController :
        BaseReadOnlyApiController<MixCmsContext, MixPost, PostViewModel>
    {
        public ApiPostClientController(DefaultRepository<MixCmsContext, MixPost, PostViewModel> repo) 
            : base(repo)
        {
        }

        [HttpGet]
        public override async Task<ActionResult<PaginationModel<PostViewModel>>> Get()
        {
            var searchPostData = new SearchPostQueryModel(Request, _lang);
            var getData = await Helper.SearchPosts<PostViewModel>(searchPostData);
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
        public async Task<ActionResult<PaginationModel<PostViewModel>>> GetByMeta()
        {
            var pagingData = new PagingRequest(Request);
            var result = await Helper.GetModelistByMeta<PostViewModel>(
                Request.Query[MixRequestQueryKeywords.DatabaseName], 
                Request.Query["value"], 
                Request.Query["postType"], 
                pagingData,
                _lang);
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
        public async Task<ActionResult<PaginationModel<PostViewModel>>> GetByValue()
        {
            var result = await Helper.SearchPostByField<PostViewModel>(
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
        public async Task<ActionResult<PaginationModel<PostViewModel>>> GetByValueId()
        {
            var result = await Mix.Cms.Lib.ViewModels.MixPosts.Helper.GetPostListByValueId<PostViewModel>(
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
        public async Task<ActionResult<PaginationModel<PostViewModel>>> GetByValueIds([FromBody] List<string> valueIds)
        {
            var result = await Mix.Cms.Lib.ViewModels.MixPosts.Helper.GetPostListByValueIds<PostViewModel>(
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
        public async Task<ActionResult<PaginationModel<PostViewModel>>> GetByAttributeDataId()
        {
            var result = await Mix.Cms.Lib.ViewModels.MixPosts.Helper.GetPostListByDataId<PostViewModel>(
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