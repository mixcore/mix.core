// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeSetDatas;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1.OData.AttributeSetDatas
{
    [Produces("application/json")]
    [Route("api/v1/odata/{culture}/attribute-set-data/mobile-full")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class ApiODataAttributeSetDataMobileFullController :
        ODataBaseApiController<MixCmsContext, MixAttributeSetData>
    {
        public ApiODataAttributeSetDataMobileFullController(
            IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/AttributeSetDatas/id
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{id}")]
        [Route("{id}/{attributeSetId}")]
        [Route("{id}/{attributeSetId}/{attributeSetName}")]
        public async Task<ActionResult<ODataMobileFullViewModel>> Details(string culture, string id, int? attributeSetId, string attributeSetName)
        {
            string msg = string.Empty;
            Expression<Func<MixAttributeSetData, bool>> predicate = null;
            MixAttributeSetData model = null;
            // Get Details if has id or else get default
            if (id != "default")
            {
                predicate = m => m.Id == id && m.Specificulture == _lang;
            }
            else
            {
                model = new MixAttributeSetData()
                {
                    Specificulture = _lang,
                };
                if (attributeSetId.HasValue)
                {
                    model.AttributeSetId = attributeSetId.Value;  
                }
                if (!string.IsNullOrEmpty(attributeSetName))
                {
                    model.AttributeSetName = attributeSetName;
                }
            }

            if (predicate != null || model != null)
            {
                var portalResult = await base.GetSingleAsync<ODataMobileFullViewModel>(id, predicate, model);
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
            return (await ODataMobileFullViewModel.Repository.CountAsync()).Data;
        }

        // Save api/odata/{culture}/attribute-set-data/portal
        [HttpPost, HttpOptions]
        [Route("")]
        public async Task<ActionResult<ODataMobileFullViewModel>> Save(string culture, [FromBody]ODataMobileFullViewModel data)
        {
            var portalResult = await base.SaveAsync<ODataMobileFullViewModel>(data, true);
            if (portalResult.IsSucceed)
            {
                return Ok(portalResult);
            }
            else
            {
                return BadRequest(portalResult);
            }
        }

        // Save api/odata/{culture}/attribute-set-data/portal
        [HttpPost, HttpOptions]
        [Route("name/{name}")]
        public async Task<ActionResult<ODataMobileFullViewModel>> Save(string culture, string name, [FromBody]ODataMobileFullViewModel data)
        {
            var getAttrSet = await Mix.Cms.Lib.ViewModels.MixAttributeSets.ReadViewModel.Repository.GetSingleModelAsync(m => m.Name == name);
            if (getAttrSet.IsSucceed)
            {
                data.AttributeSetId = getAttrSet.Data.Id;
                data.AttributeSetName = getAttrSet.Data.Name;
                data.Specificulture = culture;
                var portalResult = await base.SaveAsync<ODataMobileFullViewModel>(data, true);
                if (portalResult.IsSucceed)
                {
                    return Ok(portalResult);
                }
                else
                {
                    return BadRequest(portalResult);
                }
            }
            else
            {
                return NotFound();
            }
        }

        // Save api/odata/{culture}/attribute-set-data/portal/{id}
        [HttpPost, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<ODataMobileFullViewModel>> Save(string culture, string id, [FromBody]JObject data)
        {
            var getData = await base.GetSingleAsync<ODataMobileFullViewModel>(id, p => p.Id == id && p.Specificulture == _lang);
            getData.Data.Data = data;
            if (getData.IsSucceed)
            {
                var portalResult = await base.SaveAsync<ODataMobileFullViewModel>(getData.Data, true);
                if (portalResult.IsSucceed)
                {
                    return Ok(portalResult);
                }
                else
                {
                    return BadRequest(portalResult);
                }
            }
            else
            {
                return NotFound(data);
            }
        }

        [HttpDelete, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<ODataDeleteViewModel>> Delete(string culture, string id)
        {
            Expression<Func<MixAttributeSetData, bool>> predicate = model => model.Id == id && model.Specificulture == _lang;

            // Get Details if has id or else get default

            var portalResult = await base.GetSingleAsync<ODataDeleteViewModel>(id.ToString(), predicate);

            var result = await base.DeleteAsync<ODataDeleteViewModel>(portalResult.Data, true);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // GET api/AttributeSetDatas/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        public async Task<ActionResult<List<ODataMobileFullViewModel>>> List(string culture, ODataQueryOptions<MixAttributeSetData> queryOptions)
        {
            var data = await base.GetListAsync<ODataMobileFullViewModel>(queryOptions);
            var result = new JArray();
            if (data != null)
            {
                foreach (var item in data)
                {
                    result.Add(item.Data);
                }
            }
            return Ok(data);
        }

        // GET api/AttributeSetDatas/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        [Route("name/{name}")]
        public async Task<ActionResult<List<ODataMobileFullViewModel>>> ListByName(string culture, string name, ODataQueryOptions<MixAttributeSetData> queryOptions)
        {
            Expression<Func<MixAttributeSetData, bool>> predicate = m => m.AttributeSetName == name && m.Specificulture == culture;
            var data = await base.GetListAsync<ODataMobileFullViewModel>(predicate, queryOptions);
            var result = new JArray();
            if (data != null)
            {
                foreach (var item in data)
                {
                    result.Add(item.Data);
                }
            }
            return Ok(data);
        }

        #endregion Get

    }
}