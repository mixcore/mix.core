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

namespace Mix.Cms.Api.Controllers.OData.Positions
{

    [Produces("application/json")]
    [Route("api/odata/{culture}/position/model")]
    public class ApiODataPositionModelController :
        BaseApiODataController<MixCmsContext, MixPosition>
    {
        private readonly MixCmsContext _context;
        public ApiODataPositionModelController(
            MixCmsContext context, IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
            _context = context;

        }

        #region Get

        // GET api/Positions/id
        [AllowAnonymous]
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{id}")]
        public ActionResult<MixPosition> Details(string culture, int id)
        {
            return _context.MixPosition.SingleOrDefault(p => p.Id == id);
        }

        // GET api/Positions/id
        [AllowAnonymous]
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("")]
        public ActionResult<List<MixPosition>> List(string culture)
        {
            return _context.MixPosition.ToList();
        }
        #endregion Get

    }
}