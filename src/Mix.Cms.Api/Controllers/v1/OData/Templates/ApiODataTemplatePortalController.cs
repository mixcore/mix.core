// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixTemplates;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1.OData.Templates
{
    [Produces("application/json")]
    [Route("api/v1/odata/template/portal")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class ApiODataTemplatePortalController :
        BaseApiODataController<MixCmsContext, MixTemplate>
    {
        public ApiODataTemplatePortalController(
            IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/Templates/id
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<UpdateViewModel>> Details(string culture, int id)
        {
            string msg = string.Empty;
            Expression<Func<MixTemplate, bool>> predicate = null;
            MixTemplate model = null;
            // Get Details if has id or else get default
            if (id > 0)
            {
                predicate = m => m.Id == id;
            }
            else
            {
                model = new MixTemplate()
                {
                    Priority = UpdateViewModel.Repository.Max(p => p.Priority).Data + 1
                };
            }

            var portalResult = await base.GetSingleAsync<UpdateViewModel>(id.ToString(), predicate, model);

            return Ok(portalResult.Data);
        }

        // GET api/templates/portal/count
        [AllowAnonymous]
        [EnableQuery]
        [Route("count")]
        [HttpGet, HttpOptions]
        public async System.Threading.Tasks.Task<ActionResult<int>> CountAsync()
        {
            return (await UpdateViewModel.Repository.CountAsync()).Data;
        }

        // Save api/odata/{culture}/template/portal
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

        // Save api/odata/{culture}/template/portal/{id}
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
        public async Task<ActionResult<DeleteViewModel>> Delete(string culture, int id)
        {
            Expression<Func<MixTemplate, bool>> predicate = model => model.Id == id;

            // Get Details if has id or else get default

            var portalResult = await base.GetSingleAsync<DeleteViewModel>(id.ToString(), predicate);

            var result = await base.DeleteAsync<DeleteViewModel>(portalResult.Data, true);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // GET api/Templates/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        public async Task<ActionResult<List<UpdateViewModel>>> List(string culture, ODataQueryOptions<MixTemplate> queryOptions)
        {
            var result = await base.GetListAsync<UpdateViewModel>(queryOptions);
            return Ok(result);
        }

        #endregion Get
    }
}