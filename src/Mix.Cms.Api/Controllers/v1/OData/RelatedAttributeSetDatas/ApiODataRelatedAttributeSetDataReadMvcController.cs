// Licensed to the Mix I/O Foundation under one or more agreements.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
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
    [Route("api/v1/odata/{culture}/related-attribute-set-data/read-mvc")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class ApiODataRelatedAttributeSetDataReadMvcController :
        BaseApiODataController<MixCmsContext, MixRelatedAttributeData>
    {
        public ApiODataRelatedAttributeSetDataReadMvcController(
            IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/RelatedAttributeSetDatas/id
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{parentId}/{parentType}/{id}")]
        public async Task<ActionResult<ReadMvcViewModel>> Details(string culture, string parentId, int parentType, string id)
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
                    Priority = ReadMvcViewModel.Repository.Max(p => p.Priority).Data + 1
                };
            }

            if (predicate != null || model != null)
            {
                var portalResult = await base.GetSingleAsync<ReadMvcViewModel>(id.ToString(), predicate, model);
                return Ok(portalResult.Data);
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/AttributeSetDatas/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        [Route("parent/{parentType}/{parentId}")]
        [Route("parent/{parentType}/{parentId}/{attributeSetId}")]
        public async Task<ActionResult<List<Lib.ViewModels.MixAttributeSetDatas.ReadMvcViewModel>>> ListDataByParent(string culture, MixEnums.MixAttributeSetDataType parentType, string parentId, int? attributeSetId, ODataQueryOptions<MixRelatedAttributeData> queryOptions)
        {
            var data = new List<Lib.ViewModels.MixAttributeSetDatas.ReadMvcViewModel>();
            var arr = new JArray();
            Expression<Func<MixRelatedAttributeData, bool>> predicate = null;
            if (attributeSetId.HasValue)
            {
                predicate = m => m.ParentType == (int)parentType && m.ParentId == parentId && m.AttributeSetId == attributeSetId;
            }
            else
            {
                predicate = m => m.ParentType == (int)parentType && m.ParentId == parentId;
            }
            var result = await base.GetListAsync<ReadMvcViewModel>(predicate, $"parent_{parentType}_{parentId}_{attributeSetId}", queryOptions);
            if (result != null)
            {
                foreach (var item in result)
                {
                    data.Add(item.Data);
                    arr.Add(item.Data.Data);
                }
            }
            return Ok(arr);
        }

        // GET api/attribute-set-datas/portal/count
        [AllowAnonymous]
        [EnableQuery]
        [Route("count")]
        [HttpGet, HttpOptions]
        public async System.Threading.Tasks.Task<ActionResult<int>> CountAsync()
        {
            return (await ReadMvcViewModel.Repository.CountAsync()).Data;
        }

        // Save api/odata/{culture}/attribute-set-data/portal
        [HttpPost, HttpOptions]
        [Route("")]
        public async Task<ActionResult<ReadMvcViewModel>> Save(string culture, [FromBody]ReadMvcViewModel data)
        {
            var portalResult = await base.SaveAsync<ReadMvcViewModel>(data, true);
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
        public async Task<ActionResult<ReadMvcViewModel>> Save(string culture, string parentId, int parentType, string id, [FromBody]JObject data)
        {
            var portalResult = await base.SaveAsync<ReadMvcViewModel>(data, p => p.Id == id && p.ParentId == parentId && p.ParentType == parentType && p.Specificulture == _lang);
            if (portalResult.IsSucceed)
            {
                return Ok(portalResult);
            }
            else
            {
                return BadRequest(portalResult);
            }
        }

        // Save api/odata/{culture}/related-attribute-set-data/portal/save-properties
        // TODO: Opt Transaction
        [HttpPost, HttpOptions]
        [Route("save-properties")]
        public async Task<ActionResult<ReadMvcViewModel>> SaveProperties([FromBody]JArray data)
        {
            foreach (JObject item in data)
            {
                JObject keys = item.Value<JObject>("keys");
                JObject properties = item.Value<JObject>("properties");
                string id = keys.Value<string>("id");
                string parentId = keys.Value<string>("parentId");
                int parentType = keys.Value<int>("parentType");
                var portalResult = await base.SaveAsync<ReadMvcViewModel>(properties, p => p.Id == id && p.ParentId == parentId && p.ParentType == parentType && p.Specificulture == _lang);
                if (!portalResult.IsSucceed)
                {
                    return BadRequest(portalResult);
                }
            }
            return Ok();
        }

        [HttpDelete, HttpOptions]
        [Route("{parentId}/{parentType}/{id}")]
        public async Task<ActionResult<DeleteViewModel>> Delete(string culture, string parentId, int parentType, string id)
        {
            Expression<Func<MixRelatedAttributeData, bool>> predicate = model => model.Id == id && model.ParentId == parentId && model.ParentType == parentType && model.Specificulture == _lang;

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
        public async Task<ActionResult<List<ReadMvcViewModel>>> List(string culture, ODataQueryOptions<MixRelatedAttributeData> queryOptions)
        {
            var result = await base.GetListAsync<ReadMvcViewModel>(queryOptions);
            return Ok(result);
        }

        #endregion Get
    }
}