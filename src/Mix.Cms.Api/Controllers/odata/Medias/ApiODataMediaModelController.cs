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

namespace Mix.Cms.Api.Controllers.OData.Medias
{

    [Produces("application/json")]
    [Route("api/odata/{culture}/media/model")]
    public class ApiODataMediaModelController :
        BaseApiODataController<MixCmsContext, MixMedia>
    {
        public ApiODataMediaModelController(
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
        public ActionResult<MixMedia> Details(string culture, int id)
        {
            return _context.MixMedia.SingleOrDefault(p => p.Specificulture == culture && p.Id == id);
        }

        // GET api/Medias/id
        [AllowAnonymous]
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("")]
        public ActionResult<List<MixMedia>> List(string culture)
        {
            return _context.MixMedia.Where(p => p.Specificulture == culture).ToList();
        }
        #endregion Get

    }
}