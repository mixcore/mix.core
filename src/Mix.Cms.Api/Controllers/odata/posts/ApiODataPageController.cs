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
using Mix.Cms.Lib.ViewModels.MixPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNet.OData;
using System.Collections.Generic;

namespace Mix.Cms.Api.Controllers.OData.Pages
{
    [Produces("application/json")]
    [Route("api/odata/{culture}/page/mvc")]
    public class ApiODataPageMvcController :
        BaseApiODataController<MixCmsContext, MixPage>
    {
        public ApiODataPageMvcController(IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/pages/id
        [AllowAnonymous]
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<ReadMvcViewModel>> Details(string culture, int id)
        {
            string msg = string.Empty;
            Expression<Func<MixPage, bool>> predicate = model => model.Id == id && model.Specificulture == culture;
            var portalResult = await base.GetSingleAsync<ReadMvcViewModel>($"mvc_{id}", predicate);
            if (portalResult.IsSucceed)
            {
                portalResult.Data.DetailsUrl = MixCmsHelper.GetRouterUrl("Page", new { id = portalResult.Data.Id, SeoName = portalResult.Data.SeoName }, Request, Url);
            }
            return Ok(portalResult.Data);
        }
        
        // GET api/pages/id
        [AllowAnonymous]
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("")]
        public async Task<ActionResult<List<ReadMvcViewModel>>> List(string culture)
        {
            string msg = string.Empty;
            Expression<Func<MixPage, bool>> predicate = model => model.Specificulture == culture;
            var portalResult = await ReadMvcViewModel.Repository.GetModelListByAsync(predicate);
            if (portalResult.IsSucceed)
            {
                foreach (var item in portalResult.Data)
                {
                    item.DetailsUrl = MixCmsHelper.GetRouterUrl("Page", new { id = item.Id, SeoName = item.SeoName }, Request, Url);
                }
            }
            return Ok(portalResult.Data);
        }


        #endregion Get

    }
}