// Licensed to the Mix I/O Foundation under one or more agreements.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Mix.Cms.Api.Controllers.v1.RelatedAttributeDatas
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/related-attribute-data/portal")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class ApiRelatedAttributeSetDataPortalController :
        BaseGenericApiController<MixCmsContext, MixRelatedAttributeData>
    {
        public ApiRelatedAttributeSetDataPortalController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/RelatedAttributeSetDatas/id
        [HttpGet, HttpOptions]
        [Route("details/{parentId}/{parentType}/{id}")]
        [Route("details/{parentId}/{parentType}/{id}/{attributeSetId}")]
        public async Task<ActionResult> Details(string culture, string parentId, int parentType, string id, int? attributeSetId)
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
                if (attributeSetId.HasValue)
                {
                    model.AttributeSetId = attributeSetId.Value;
                }
            }

            if (predicate != null || model != null)
            {
                var portalResult = await base.GetSingleAsync<UpdateViewModel>(predicate, model);
                return Ok(portalResult);
            }
            else
            {
                return NotFound();
            }
        }

        // Save api//{culture}/attribute-set-data/portal
        [HttpPost, HttpOptions]
        [Route("")]
        public async Task<ActionResult> Save(string culture, [FromBody]UpdateViewModel data)
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

        // Save api//{culture}/attribute-set-data/portal/{id}
        [HttpPost, HttpOptions]
        [Route("{parentId}/{parentType}/{id}")]
        public async Task<ActionResult> Save(string culture, string parentId, int parentType, string id, [FromBody]JObject data)
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

        // Save api//{culture}/related-attribute-set-data/portal/save-properties
        // TODO: Opt Transaction
        [HttpPost, HttpOptions]
        [Route("save-properties")]
        public async Task<ActionResult> SaveProperties([FromBody]JArray data)
        {
            foreach (JObject item in data)
            {
                JObject keys = item.Value<JObject>("keys");
                JObject properties = item.Value<JObject>("properties");
                string id = keys.Value<string>("id");
                string parentId = keys.Value<string>("parentId");
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
        public async Task<ActionResult> Delete(string culture, string parentId, int parentType, string id)
        {
            Expression<Func<MixRelatedAttributeData, bool>> predicate = model => model.Id == id && model.ParentId == parentId && model.ParentType == parentType && model.Specificulture == _lang;

            // Get Details if has id or else get default
            var portalResult = await base.GetSingleAsync<DeleteViewModel>(predicate);

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

        // GET api/related-attribute-set
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult> GetList(
            [FromBody] RequestPaging request)
        {
            var parsed = HttpUtility.ParseQueryString(request.Query ?? "");
            ParseRequestPagingDate(request);
            string parentId = parsed.Get("parentId");
            int.TryParse(parsed.Get("parentType"), out int parentType);
            int.TryParse(parsed.Get("attributeSetId"), out int attributeSetId);

            Expression<Func<MixRelatedAttributeData, bool>> predicate = model =>
                        (!request.Status.HasValue || model.Status == request.Status.Value)
                        && (string.IsNullOrWhiteSpace(parentId)
                            || (model.ParentId == parentId)
                            )
                        && (string.IsNullOrWhiteSpace(parentId)
                            || (model.ParentId == parentId)
                            )
                        && (parentType == 0
                            || (model.ParentType == parentType)
                            )
                        && (attributeSetId == 0
                            || (model.AttributeSetId == attributeSetId)
                            )
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (model.Description.Contains(request.Keyword))
                            )
                        && (!request.FromDate.HasValue
                            || (model.CreatedDateTime >= request.FromDate.Value)
                        )
                        && (!request.ToDate.HasValue
                            || (model.CreatedDateTime <= request.ToDate.Value)
                        );

            switch (request.Key)
            {
                case "portal":
                    var portalResult = await base.GetListAsync<UpdateViewModel>(request, predicate);
                    return Ok(JObject.FromObject(portalResult));

                case "mvc":
                    var mvcResult = await base.GetListAsync<ReadMvcViewModel>(request, predicate);
                    return Ok(JObject.FromObject(mvcResult));

                default:

                    var listItemResult = await base.GetListAsync<ReadViewModel>(request, predicate);
                    return Ok(JObject.FromObject(listItemResult));
            }
        }

        #endregion Get
    }
}