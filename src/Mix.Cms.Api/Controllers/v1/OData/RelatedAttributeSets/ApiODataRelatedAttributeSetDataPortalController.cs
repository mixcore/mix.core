// Licensed to the Mix I/O Foundation under one or more agreements.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixRelatedAttributeSets;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1.OData.RelatedAttributeSets
{
    [Produces("application/json")]
    [Route("api/v1/odata/{culture}/related-attribute-set/portal")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class ApiODataRelatedAttributeSetPortalController :
        BaseApiODataController<MixCmsContext, MixRelatedAttributeSet>
    {
        public ApiODataRelatedAttributeSetPortalController(
            IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/RelatedAttributeSets/id
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{parentId}/{parentType}/{id}")]
        public async Task<ActionResult<UpdateViewModel>> Details(string culture, int parentId, int parentType, int id)
        {
            string msg = string.Empty;
            Expression<Func<MixRelatedAttributeSet, bool>> predicate = null;
            MixRelatedAttributeSet model = null;
            // Get Details if has id or else get default
            if (id > 0)
            {
                predicate = m => m.Id == id && m.ParentId == parentId && m.ParentType == parentType && m.Specificulture == _lang;
            }
            else
            {
                model = new MixRelatedAttributeSet()
                {
                    Specificulture = _lang,
                    ParentType = parentType,
                    ParentId = parentId,
                    Priority = UpdateViewModel.Repository.Max(p => p.Priority).Data + 1
                };
            }

            if (predicate != null || model != null)
            {
                var portalResult = await base.GetSingleAsync<UpdateViewModel>(id.ToString(), predicate, model);
                return Ok(portalResult.Data);
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/attribute-sets/portal/count
        [AllowAnonymous]
        [EnableQuery]
        [Route("count")]
        [HttpGet, HttpOptions]
        public async System.Threading.Tasks.Task<ActionResult<int>> CountAsync()
        {
            return (await UpdateViewModel.Repository.CountAsync()).Data;
        }

        // Save api/odata/{culture}/attribute-set/portal
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

        // Save api/odata/{culture}/attribute-set/portal/{id}
        [HttpPost, HttpOptions]
        [Route("{parentId}/{parentType}/{id}")]
        public async Task<ActionResult<UpdateViewModel>> Save(string culture, int parentId, int parentType, int id, [FromBody]JObject data)
        {
            var portalResult = await base.SaveAsync<UpdateViewModel>(data, p => p.Id == id && p.ParentId == parentId && p.ParentType == parentType && p.Specificulture == _lang);
            if (portalResult.IsSucceed)
            {
                return Ok(portalResult);
            }
            else
            {
                return BadRequest(portalResult);
            }
        }

        // Save api/odata/{culture}/related-attribute-set/portal/save-properties
        // TODO: Opt Transaction
        [HttpPost, HttpOptions]
        [Route("save-properties")]
        public async Task<ActionResult<UpdateViewModel>> SaveProperties([FromBody]JArray data)
        {
            foreach (JObject item in data)
            {
                JObject keys = item.Value<JObject>("keys");
                JObject properties = item.Value<JObject>("properties");
                int id = keys.Value<int>("id");
                int parentId = keys.Value<int>("parentId");
                int parentType = keys.Value<int>("parentType");
                var portalResult = await base.SaveAsync<UpdateViewModel>(properties, p => p.Id == id && p.ParentId == parentId && p.ParentType == parentType && p.Specificulture == _lang);
                if (!portalResult.IsSucceed)
                {
                    return BadRequest(portalResult);
                }
            }
            return Ok();
        }

        [HttpDelete, HttpOptions]
        [Route("{parentId}/{parentType}/{id}")]
        public async Task<ActionResult<DeleteViewModel>> Delete(string culture, int parentId, int parentType, int id)
        {
            Expression<Func<MixRelatedAttributeSet, bool>> predicate = model => model.Id == id && model.ParentId == parentId && model.ParentType == parentType && model.Specificulture == _lang;

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

        // GET api/RelatedAttributeSets/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        public async Task<ActionResult<List<UpdateViewModel>>> List(string culture, ODataQueryOptions<MixRelatedAttributeSet> queryOptions)
        {
            var result = await base.GetListAsync<UpdateViewModel>(queryOptions);
            return Ok(result);
        }

        #endregion Get
    }
}