// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib;
using System.Linq.Expressions;
using Mix.Cms.Lib.ViewModels.MixPosts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNet.OData;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Query;
using Mix.Cms.Lib.Helpers;

namespace Mix.Cms.Api.Controllers.OData.Posts
{
    [Produces("application/json")]
    [Route("api/odata/{culture}/post/mvc")]
    public class ApiODataPostMvcController :
        BaseApiODataController<MixCmsContext, MixPost>
    {
        public ApiODataPostMvcController(MixCmsContext context, IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/pages/id
        [AllowAnonymous]
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<ReadMvcViewModel>> Details(string culture, int id)
        {
            string msg = string.Empty;
            Expression<Func<MixPost, bool>> predicate = model => model.Id == id && model.Specificulture == culture;
            var portalResult = await base.GetSingleAsync<ReadMvcViewModel>($"mvc_{id}", predicate);
            if (portalResult.IsSucceed)
            {
                portalResult.Data.DetailsUrl = MixCmsHelper.GetRouterUrl("Post", new { id = portalResult.Data.Id, SeoName = portalResult.Data.SeoName }, Request, Url);
            }
            return Ok(portalResult.Data);
        }

        // GET api/pages/id
        [AllowAnonymous]
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        [Route("")]
        public async Task<ActionResult<List<ReadMvcViewModel>>> List(string culture, ODataQueryOptions<MixPost> queryOptions)
        {
            string msg = string.Empty;
            var portalResult = await base.GetListAsync<ReadMvcViewModel>(queryOptions);
            if (portalResult != null)
            {
                foreach (var item in portalResult)
                {
                    item.DetailsUrl = MixCmsHelper.GetRouterUrl("Page", new { id = item.Id, SeoName = item.SeoName }, Request, Url);
                }
            }
            return Ok(portalResult);
        }


        #endregion Get
    }
}