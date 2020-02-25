// Licensed to the Mix I/O Foundation under one or more agreements.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixRelatedAttributeSets;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Mix.Cms.Api.Controllers.v1.RelatedAttributeSets
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/related-attribute-set/portal")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class ApiRelatedAttributeSetPortalController :
        BaseGenericApiController<MixCmsContext, MixRelatedAttributeSet>
    {
        public ApiRelatedAttributeSetPortalController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/related-attribute-set//id
        [HttpGet, HttpOptions]
        [Route("details/{parentId}/{parentType}/{id}")]
        public async Task<ActionResult> Details(int parentId, int parentType, int id)
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
                return Ok(portalResult);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet, HttpOptions]
        [Route("delete/{parentId}/{parentType}/{id}")]
        public async Task<ActionResult> Delete(int parentId, int parentType, int id)
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

        #endregion Get

        #region Post

        // Save api/odata/{culture}/related-attribute-set/portal
        [HttpPost, HttpOptions]
        [Route("save")]
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

        // Save api/odata/{culture}/related-attribute-set/portal/{id}
        [HttpPost, HttpOptions]
        [Route("{parentId}/{parentType}/{id}")]
        public async Task<ActionResult> Save(string culture, int parentId, int parentType, int id, [FromBody]JObject data)
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

        // Save api/odata/{culture}/related-related-attribute-set/portal/save-properties
        // TODO: Opt Transaction
        [HttpPost, HttpOptions]
        [Route("save-properties")]
        public async Task<ActionResult> SaveProperties([FromBody]JArray data)
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

        // GET api/related-attribute-set
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult> GetList(
            [FromBody] RequestPaging request)
        {
            var parsed = HttpUtility.ParseQueryString(request.Query ?? "");
            int.TryParse(parsed.Get("parentType"), out int parentType);
            int.TryParse(parsed.Get("parentId"), out int parentId);
            ParseRequestPagingDate(request);
            Expression<Func<MixRelatedAttributeSet, bool>> predicate = model =>
                        (!request.Status.HasValue || model.Status == request.Status.Value)
                        && (parentId == 0
                            || (model.ParentId == parentId)
                            )
                        && (parentType == 0
                            || (model.ParentType == parentType)
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

        #endregion Post
    }
}