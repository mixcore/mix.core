// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixPagePosts;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/page-post/mvc")]
    public class ApiPagePostMvcController :
        BaseRestApiController<MixCmsContext, MixPagePost, ReadMvcViewModel>
    {
        public ApiPagePostMvcController(DefaultRepository<MixCmsContext, MixPagePost, ReadMvcViewModel> repo) 
            : base(repo)
        {
        }

        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadMvcViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            bool isPage = int.TryParse(Request.Query["page_id"], out int pageId);
            bool isPost = int.TryParse(Request.Query["post_id"], out int postId);
            string keyword = Request.Query[MixRequestQueryKeywords.Keyword];
            Expression<Func<MixPagePost, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (!isStatus || model.Status == status)
                        && (!isFromDate || model.CreatedDateTime >= fromDate)
                        && (!isToDate || model.CreatedDateTime <= toDate)
                        && (!isPage || model.PageId == pageId)
                        && (!isPost || model.PostId == postId)
                        && (string.IsNullOrWhiteSpace(keyword)
                            || (model.Description.Contains(keyword)
                            ));
            var getData = await base.GetListAsync<ReadMvcViewModel>(predicate);
            if (getData.IsSucceed)
            {
                return getData.Data;
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }
    }
}