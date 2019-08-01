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
    [Route("api/odata/{culture}/post/model")]
    public class ApiODataPostModelController :
        BaseApiODataController<MixCmsContext, MixArticle>
    {
        private readonly MixCmsContext _context;
        public ApiODataPostModelController(
            MixCmsContext context, IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
            this._context = context;
        }

        #region Get

        // GET api/pages/id
        [AllowAnonymous]
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{id}")]
        public ActionResult<MixArticle> Details(string culture, int id)
        {
            return _context.MixArticle.SingleOrDefault(p => p.Specificulture == culture && p.Id == id);
        }
        
        // GET api/pages/id
        [AllowAnonymous]
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("")]
        public ActionResult<List<MixArticle>> List(string culture)
        {
            return _context.MixArticle.Where(p => p.Specificulture == culture).ToList();
        }


        #endregion Get
    }
}