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
using Mix.Cms.Lib.ViewModels.MixPositions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNet.OData;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Linq;

namespace Mix.Cms.Api.Controllers.OData.Positions
{
    [Produces("application/json")]
    [Route("api/odata/{culture}/position/portal")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class ApiODataPositionMvcController :
        BaseApiODataController<MixCmsContext, MixPosition>
    {
        public ApiODataPositionMvcController(IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/Positions/id
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<UpdateViewModel>> Details(string culture, int id)
        {
            string msg = string.Empty;
            Expression<Func<MixPosition, bool>> predicate = null;
            MixPosition model = null;
            // Get Details if has id or else get default
            if (id > 0)
            {
                predicate = m => m.Id == id;
                model = new MixPosition()
                {
                    Priority = UpdateViewModel.Repository.Max(p => p.Priority).Data + 1
                };
            }

            var portalResult = await base.GetSingleAsync<UpdateViewModel>(id.ToString(), predicate, model)
            ;

            if (portalResult.IsSucceed)
            {
                await portalResult.Data.LoadPageAsync(_lang);
            }
            return Ok(portalResult.Data);
        }

        // Save api/odata/{culture}/position/portal
        [HttpPost, HttpOptions]
        [Route("")]
        public async Task<ActionResult<UpdateViewModel>> Save(string culture, [FromBody]UpdateViewModel data)
        {
            var portalResult = await base.SaveAsync<UpdateViewModel>(data, true);
            if (portalResult.IsSucceed)
            {
                return Ok(portalResult);
            }
            else
            {
                return BadRequest(portalResult);
            }
        }
        
        // Save api/odata/{culture}/position/portal/{id}
        [HttpPost, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<UpdateViewModel>> Save(string culture, int id, [FromBody]JObject data)
        {
            var portalResult = await base.SaveAsync<UpdateViewModel>(data, p => p.Id == id);
            if (portalResult.IsSucceed)
            {
                return Ok(portalResult);
            }
            else
            {
                return BadRequest(portalResult);
            }
        }

        [HttpDelete, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<UpdateViewModel>> Delete(string culture, int id)
        {
            Expression<Func<MixPosition, bool>> predicate = model => model.Id == id;

            // Get Details if has id or else get default

            var portalResult = await base.GetSingleAsync<UpdateViewModel>(id.ToString(), predicate);

            if (portalResult.IsSucceed)
            {
                await portalResult.Data.LoadPageAsync(_lang);
            }

            var result = await base.DeleteAsync<UpdateViewModel>(portalResult.Data, true);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // GET api/Positions/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        [Route("")]
        public async Task<ActionResult<List<UpdateViewModel>>> List(string culture, ODataQueryOptions<MixPosition> queryOptions)
        {
            var result = await base.GetListAsync<UpdateViewModel>(queryOptions);
            if (result != null)
            {
                foreach (var item in result)
                {
                    await item.LoadPageAsync(_lang);
                }
            }
            return Ok(result);
        }

        #endregion Get

    }
}