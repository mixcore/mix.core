// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Lib.ViewModels.MixAttributeSets;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1.OData.AttributeSets
{
    [Produces("application/json")]
    [Route("api/v1/odata/attribute-set/read")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class ApiODataAttributeSetReadController :
        BaseApiODataController<MixCmsContext, MixAttributeSet>
    {
        public ApiODataAttributeSetReadController(
            IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/AttributeSets/id
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<ReadViewModel>> Details(string culture, int id)
        {
            string msg = string.Empty;
            Expression<Func<MixAttributeSet, bool>> predicate = null;
            MixAttributeSet model = null;
            // Get Details if has id or else get default
            if (id > 0)
            {
                predicate = m => m.Id == id;
            }
            else
            {
                model = new MixAttributeSet()
                {
                    Priority = ReadViewModel.Repository.Max(p => p.Priority).Data + 1
                };
            }

            var portalResult = await base.GetSingleAsync<ReadViewModel>(id.ToString(), predicate, model);

            return Ok(portalResult.Data);
        }

        // GET api/attribute-sets/portal/count
        [AllowAnonymous]
        [EnableQuery]
        [Route("count")]
        [HttpGet, HttpOptions]
        public async System.Threading.Tasks.Task<ActionResult<int>> CountAsync()
        {
            return (await ReadViewModel.Repository.CountAsync()).Data;
        }

        // Save api/odata/{culture}/attribute-set/portal
        [HttpPost, HttpOptions]
        [Route("")]
        public async Task<ActionResult<ReadViewModel>> Save(string culture, [FromBody]ReadViewModel data)
        {
            var portalResult = await base.SaveAsync<ReadViewModel>(data, true);
            if (portalResult.IsSucceed)
            {
                return Ok(portalResult);
            }
            else
            {
                return BadRequest(portalResult);
            }
        }

        // Save api/odata/{culture}/attribute-set/portal/{id}
        [HttpPost, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<ReadViewModel>> Save(string culture, int id, [FromBody]JObject data)
        {
            var portalResult = await base.SaveAsync<ReadViewModel>(data, p => p.Id == id);
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
        public async Task<ActionResult<ReadViewModel>> Delete(string culture, int id)
        {
            Expression<Func<MixAttributeSet, bool>> predicate = model => model.Id == id;

            // Get Details if has id or else get default

            var portalResult = await base.GetSingleAsync<ReadViewModel>(id.ToString(), predicate);

            var result = await base.DeleteAsync<ReadViewModel>(portalResult.Data, true);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // GET api/AttributeSets/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        public async Task<ActionResult<List<ReadViewModel>>> List(string culture, ODataQueryOptions<MixAttributeSet> queryOptions)
        {
            var result = await base.GetListAsync<ReadViewModel>(queryOptions);
            return Ok(result);
        }

        #endregion Get

        #region Post

        [HttpPost, HttpOptions]
        [Route("apply-list")]
        public async Task<ActionResult<JObject>> ListActionAsync([FromBody]ListAction<int> data)
        {
            Expression<Func<MixAttributeSet, bool>> predicate = model =>
                       data.Data.Contains(model.Id);
            var result = new RepositoryResponse<bool>();
            switch (data.Action)
            {
                case "Delete":
                    return Ok(JObject.FromObject(await base.DeleteListAsync<DeleteViewModel>(predicate, true)));

                case "Export":
                    return Ok(JObject.FromObject(await base.ExportListAsync(predicate, MixEnums.MixStructureType.AttributeSet)));

                default:
                    return JObject.FromObject(new RepositoryResponse<bool>());
            }
        }

        #endregion Post
    }
}