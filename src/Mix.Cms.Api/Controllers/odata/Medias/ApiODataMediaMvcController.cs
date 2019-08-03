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
using Mix.Cms.Lib.ViewModels.MixMedias;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNet.OData;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Query;

namespace Mix.Cms.Api.Controllers.OData.Medias
{
    [Produces("application/json")]
    [Route("api/odata/{culture}/Media/mvc")]
    public class ApiODataMediaMvcController :
        BaseApiODataController<MixCmsContext, MixMedia>
    {
        public ApiODataMediaMvcController(
            MixCmsContext context, IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/Medias/id
        [AllowAnonymous]
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<UpdateViewModel>> Details(string culture, int id)
        {
            string msg = string.Empty;
            Expression<Func<MixMedia, bool>> predicate = model => model.Id == id && model.Specificulture == culture;
            var portalResult = await base.GetSingleAsync<UpdateViewModel>($"mvc_{id}", predicate);
            return Ok(portalResult.Data);
        }
        
        // GET api/Medias/id
        [AllowAnonymous]
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        [Route("")]
        public async Task<ActionResult<List<UpdateViewModel>>> List(string culture, ODataQueryOptions<MixMedia> queryOptions)
        {
            string msg = string.Empty;
            var portalResult = await base.GetListAsync<UpdateViewModel>(queryOptions);
            return Ok(portalResult);
        }

        #endregion Get

    }
}