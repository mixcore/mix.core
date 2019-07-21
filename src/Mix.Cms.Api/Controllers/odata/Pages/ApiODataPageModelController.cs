// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Models.Cms;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNet.OData;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Api.Controllers.OData.Pages
{
    [Produces("application/json")]
    [Route("api/odata/{culture}/page/model")]
    public class ApiODataPageModelController :
        BaseApiODataController<MixCmsContext, MixPage>
    {
        private readonly MixCmsContext _context;
        public ApiODataPageModelController(
            MixCmsContext context, IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
            _context = context;
        }

        #region Get

        // GET api/pages/id
        [AllowAnonymous]
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{id}")]
        public ActionResult<MixPage> Details(string culture, int id)
        {
            return _context.MixPage.SingleOrDefault(p => p.Specificulture == culture && p.Id == id);
        }
        
        // GET api/pages/id
        [AllowAnonymous]
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("")]
        public ActionResult<List<MixPage>> List(string culture)
        {
            return _context.MixPage.Where(p => p.Specificulture == culture).ToList();
        }


        #endregion Get

    }
}