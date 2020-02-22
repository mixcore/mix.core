// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1.OData.RelatedAttributeSetDatas
{
    [Produces("application/json")]
    [Route("api/v1/odata/{culture}/related-attribute-set-data/read")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class ApiODataRelatedAttributeSetDataReadController :
        BaseApiODataController<MixCmsContext, MixRelatedAttributeData>
    {
        public ApiODataRelatedAttributeSetDataReadController(
            IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/RelatedAttributeSetDatas/id
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{id}/{desId}")]
        public async Task<ActionResult<ReadViewModel>> Details(string culture, string id, string desId)
        {
            string msg = string.Empty;
            Expression<Func<MixRelatedAttributeData, bool>> predicate = null;
            MixRelatedAttributeData model = null;
            // Get Details if has id or else get default
            if (id != "default")
            {
                predicate = m => m.Id == id && m.ParentId == desId && m.Specificulture == _lang;
            }
            else
            {
                model = new MixRelatedAttributeData()
                {
                    Specificulture = _lang,
                    Priority = ReadViewModel.Repository.Max(p => p.Priority).Data + 1
                };
            }

            var portalResult = await base.GetSingleAsync<ReadViewModel>(id.ToString(), predicate, model);

            return Ok(portalResult.Data);
        }

        // GET api/attribute-set-datas/portal/count
        [AllowAnonymous]
        [EnableQuery]
        [Route("count")]
        [HttpGet, HttpOptions]
        public async System.Threading.Tasks.Task<ActionResult<int>> CountAsync()
        {
            return (await ReadViewModel.Repository.CountAsync()).Data;
        }

        // Save api/odata/{culture}/attribute-set-data/portal
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

        // Save api/odata/{culture}/attribute-set-data/portal/{id}
        [HttpPost, HttpOptions]
        [Route("{id}/{desId}")]
        public async Task<ActionResult<ReadViewModel>> Save(string culture, string id, string desId, [FromBody]JObject data)
        {
            var portalResult = await base.SaveAsync<ReadViewModel>(data, p => p.Id == id && p.ParentId == desId && p.Specificulture == _lang);
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
        [Route("{id}/{desId}")]
        public async Task<ActionResult<ReadViewModel>> Delete(string culture, string id, string desId)
        {
            Expression<Func<MixRelatedAttributeData, bool>> predicate = model => model.Id == id && model.ParentId == desId && model.Specificulture == _lang;

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

        // GET api/RelatedAttributeSetDatas/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        public async Task<ActionResult<List<ReadViewModel>>> List(string culture, ODataQueryOptions<MixRelatedAttributeData> queryOptions)
        {
            var result = await base.GetListAsync<ReadViewModel>(queryOptions);
            return Ok(result);
        }

        #endregion Get
    }
}