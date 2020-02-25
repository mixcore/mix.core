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
using Mix.Cms.Lib.ViewModels.MixAttributeSetDatas;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1.OData.AttributeSetDatas
{
    [Produces("application/json")]
    [Route("api/v1/odata/{culture}/attribute-set-data/read")]
    [AllowAnonymous]
    public class ApiODataAttributeSetDataReadController :
        BaseApiODataController<MixCmsContext, MixAttributeSetData>
    {
        public ApiODataAttributeSetDataReadController(
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
        public async Task<ActionResult<ReadViewModel>> Details(string culture, string id, int attributeSetId)
        {
            string msg = string.Empty;
            Expression<Func<MixAttributeSetData, bool>> predicate = null;
            MixAttributeSetData model = null;
            // Get Details if has id or else get default
            predicate = m => m.Id == id && m.Specificulture == _lang;

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

        // GET api/attribute-set-datas/portal/count
        [AllowAnonymous]
        [EnableQuery]
        [Route("count/{name}")]
        [HttpGet, HttpOptions]
        public async System.Threading.Tasks.Task<ActionResult<int>> CountByName(string name)
        {
            return (await ReadViewModel.Repository.CountAsync(m => m.AttributeSetName == name && m.Specificulture == _lang)).Data;
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
        [Route("{id}")]
        public async Task<ActionResult<ReadViewModel>> Save(string culture, string id, [FromBody]JObject data)
        {
            var portalResult = await base.SaveAsync<ReadViewModel>(data, p => p.Id == id && p.Specificulture == _lang);
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
        public async Task<ActionResult<ReadViewModel>> Delete(string culture, string id)
        {
            Expression<Func<MixAttributeSetData, bool>> predicate = model => model.Id == id && model.Specificulture == _lang;

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

        // GET api/AttributeSetDatas/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        public async Task<ActionResult<List<ReadViewModel>>> List(string culture, ODataQueryOptions<MixAttributeSetData> queryOptions)
        {
            var result = await base.GetListAsync<ReadViewModel>(queryOptions);
            return Ok(result);
        }

        #endregion Get

        [HttpPost, HttpOptions]
        [Route("apply-list")]
        public async Task<ActionResult<JObject>> ListActionAsync([FromBody]ListAction<string> data)
        {
            Expression<Func<MixAttributeSetData, bool>> predicate = model =>
                       model.Specificulture == _lang
                       && data.Data.Contains(model.Id);
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
    }
}