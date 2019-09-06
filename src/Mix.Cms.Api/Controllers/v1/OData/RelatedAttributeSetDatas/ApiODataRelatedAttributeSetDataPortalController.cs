// Licensed to the Mix I/O Foundation under one or more agreements.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib;
using System.Linq.Expressions;
using Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNet.OData;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace Mix.Cms.Api.Controllers.v1.OData.RelatedAttributeSetDatas
{
    [Produces("application/json")]
    [Route("api/v1/odata/{culture}/related-attribute-set-data/portal")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class ApiODataRelatedAttributeSetDataPortalController :
        BaseApiODataController<MixCmsContext, MixRelatedAttributeData>
    {
        public ApiODataRelatedAttributeSetDataPortalController(
            IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/RelatedAttributeSetDatas/id
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{parentId}/{parentType}/{id}")]
        public async Task<ActionResult<UpdateViewModel>> Details(string culture, string parentId, int parentType, string id)
        {
            string msg = string.Empty;
            Expression<Func<MixRelatedAttributeData, bool>> predicate = null;
            MixRelatedAttributeData model = null;
            // Get Details if has id or else get default
            if (id != "default")
            {
                predicate = m => m.Id == id && m.ParentId == parentId && m.ParentType == parentType && m.Specificulture == _lang; 
            }
            else
            {
                model = new MixRelatedAttributeData()
                {
                    Specificulture = _lang,
                    ParentType = parentType,
                    ParentId = parentId,
                    Priority = UpdateViewModel.Repository.Max(p => p.Priority).Data + 1
                };
            }

            if(predicate!=null || model != null)
            {
                var portalResult = await base.GetSingleAsync<UpdateViewModel>(id.ToString(), predicate, model);
                return Ok(portalResult.Data);
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/attribute-set-datas/portal/count
        [AllowAnonymous]
        [EnableQuery]
        [Route("count")]
        [HttpGet, HttpOptions]
        public async System.Threading.Tasks.Task<ActionResult<int>> CountAsync()
        {
            return (await UpdateViewModel.Repository.CountAsync()).Data;
        }

        // Save api/odata/{culture}/attribute-set-data/portal
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
        
        // Save api/odata/{culture}/attribute-set-data/portal/{id}
        [HttpPost, HttpOptions]
        [Route("{parentId}/{parentType}/{id}")]
        public async Task<ActionResult<UpdateViewModel>> Save(string culture, string parentId, int parentType, string id, [FromBody]JObject data)
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

        [HttpDelete, HttpOptions]
        [Route("{parentId}/{parentType}/{id}")]
        public async Task<ActionResult<DeleteViewModel>> Delete(string culture, string parentId, int parentType, string id)
        {
            Expression<Func<MixRelatedAttributeData, bool>> predicate = model => model.Id == id && model.ParentId == parentId && model.ParentType== parentType && model.Specificulture == _lang;

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

        // GET api/RelatedAttributeSetDatas/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        public async Task<ActionResult<List<UpdateViewModel>>> List(string culture, ODataQueryOptions<MixRelatedAttributeData> queryOptions)
        {
            var result = await base.GetListAsync<UpdateViewModel>(queryOptions);
            return Ok(result);
        }

        #endregion Get

    }
}