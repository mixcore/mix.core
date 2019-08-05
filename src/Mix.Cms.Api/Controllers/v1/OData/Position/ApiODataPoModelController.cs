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
using Microsoft.AspNet.OData.Routing;
using Microsoft.EntityFrameworkCore;

namespace Mix.Cms.Api.Controllers.v1.OData.Positions
{

    [Produces("application/json")]
    [Route("api/v1/odata/{culture}/position/model")]
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
        [Route("{id}")]
        [HttpGet, HttpOptions]
        public ActionResult<MixPosition> Details(string culture, [FromODataUri]int id)
        {
            if (id > 0)
            {
                return _context.MixPosition.SingleOrDefault(p => p.Id == id);
            }
            else
            {
                return Ok(new MixPosition());
            }
            
        }
        
        // GET api/Positions/id
        [AllowAnonymous]
        [EnableQuery]
        [Route("count")]
        [HttpGet, HttpOptions]
        public async System.Threading.Tasks.Task<ActionResult<int>> CountAsync(string culture)
        {
            return await _context.MixPosition.CountAsync();
        }

        // GET api/Positions/id
        [AllowAnonymous]
        [EnableQuery]
        [HttpGet, HttpOptions]
        public ActionResult<List<MixPosition>> List([FromODataUri]string culture)
        {
            return _context.MixPosition.ToList();
        }
        #endregion Get
    }
}