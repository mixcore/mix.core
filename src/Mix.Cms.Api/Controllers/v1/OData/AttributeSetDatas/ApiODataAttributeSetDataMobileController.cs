// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixAttributeSetDatas;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1.OData.AttributeSetDatas
{
    [Produces("application/json")]
    [Route("api/v1/odata/{culture}/attribute-set-data/mobile")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiODataAttributeSetDataMobileController :
        ODataBaseApiController<MixCmsContext, MixAttributeSetData>
    {
        public ApiODataAttributeSetDataMobileController(
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
        public async Task<ActionResult<ODataMobileViewModel>> Details(string culture, string id, int? attributeSetId, string attributeSetName)
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
                var portalResult = await base.GetSingleAsync<ODataMobileViewModel>(predicate, model);
                if (portalResult.IsSucceed)
                {
                    RepositoryResponse<JObject> result = new RepositoryResponse<JObject>()
                    {
                        IsSucceed = true,
                        Data = portalResult.Data.Data
                    };
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
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
            return (await ODataMobileViewModel.Repository.CountAsync()).Data;
        }

        [HttpPost, HttpOptions]
        [Route("")]
        public async Task<ActionResult<ODataMobileViewModel>> Save(string culture, [FromBody]JObject data)
        {
            string id = data["id"]?.Value<string>();
            string _username = User?.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
            if (!string.IsNullOrEmpty(id))
            {
                var getData = await base.GetSingleAsync<ODataMobileViewModel>(p => p.Id == id && p.Specificulture == _lang);
                if (getData.IsSucceed)
                {
                    if (string.IsNullOrEmpty(getData.Data.CreatedBy) || getData.Data.CreatedBy == _username)
                    {
                        getData.Data.Data = data;
                        getData.Data.CreatedBy = User?.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
                        var portalResult = await base.SaveAsync<ODataMobileViewModel>(getData.Data, true);
                        if (portalResult.IsSucceed)
                        {
                            return Ok(portalResult);
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // Save api/odata/{culture}/attribute-set-data/portal
        [AllowAnonymous]
        [HttpPost, HttpOptions]
        [Route("name/{name}")]
        public async Task<ActionResult<ODataMobileViewModel>> SaveByName(string culture, string name, [FromBody]JObject obj)
        {
            var getAttrSet = await Mix.Cms.Lib.ViewModels.MixAttributeSets.ReadViewModel.Repository.GetSingleModelAsync(m => m.Name == name);
            string _username = User?.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
            if (getAttrSet.IsSucceed)
            {
                ODataMobileViewModel data = new ODataMobileViewModel()
                {
                    Id = obj["id"]?.Value<string>(),
                    CreatedBy = _username,
                    AttributeSetId = getAttrSet.Data.Id,
                    AttributeSetName = getAttrSet.Data.Name,
                    Specificulture = culture,
                    Data = obj
                };
                var portalResult = await base.SaveAsync<ODataMobileViewModel>(data, true);
                if (portalResult.IsSucceed)
                {
                    if (getAttrSet.Data.EdmAutoSend.HasValue && getAttrSet.Data.EdmAutoSend.Value)
                    {
                        _ = MixService.SendEdm(_lang, getAttrSet.Data.EdmTemplate, portalResult.Data.Data, getAttrSet.Data.EdmSubject, getAttrSet.Data.EdmFrom);
                    }

                    return Ok(new RepositoryResponse<JObject>
                    {
                        IsSucceed = true,
                        Data = portalResult.Data.Data
                    });
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
        [AllowAnonymous]
        [HttpPost, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<ODataMobileViewModel>> Save(string culture, string id, [FromBody]JObject data)
        {
            var getData = await base.GetSingleAsync<ODataMobileViewModel>(p => p.Id == id && p.Specificulture == _lang);

            if (getData.IsSucceed)
            {
                getData.Data.Data = data;
                var portalResult = await base.SaveAsync<ODataMobileViewModel>(getData.Data, true);
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
            var portalResult = await base.GetSingleAsync<ODataDeleteViewModel>(predicate);

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
        [AllowAnonymous]
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        public async Task<ActionResult<List<ODataMobileViewModel>>> List(string culture, ODataQueryOptions<MixAttributeSetData> queryOptions)
        {
            var data = await base.GetListAsync<ODataMobileViewModel>(queryOptions);
            var result = new JArray();
            if (data != null)
            {
                foreach (var item in data)
                {
                    result.Add(item.Data);
                }
            }
            return Ok(new RepositoryResponse<JArray>
            {
                IsSucceed = true,
                Data = result
            });
        }

        // GET api/AttributeSetDatas/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("name/{name}")]
        public async Task<ActionResult<List<ODataMobileViewModel>>> ListByName(string culture, string name, ODataQueryOptions<MixAttributeSetData> queryOptions)
        {
            Expression<Func<MixAttributeSetData, bool>> predicate = m => m.AttributeSetName == name && m.Specificulture == culture;
            var data = await base.GetListAsync<ODataMobileViewModel>(predicate, queryOptions);
            var result = new JArray();
            if (data != null)
            {
                foreach (var item in data)
                {
                    result.Add(item.Data);
                }
            }
            return Ok(new RepositoryResponse<JArray>
            {
                IsSucceed = true,
                Data = result
            });
        }

        // GET api/AttributeSetDatas/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("filter/{name}")]
        public async Task<ActionResult<List<ODataMobileViewModel>>> FilterByValue(string culture, string name, ODataQueryOptions<MixAttributeSetData> queryOptions)
        {
            var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(Request.QueryString.Value);
            var data = await Lib.ViewModels.MixAttributeSetDatas.Helper.FilterByValueAsync<ODataMobileViewModel>(culture, name, queryDictionary);
            var result = new JArray();
            if (data != null)
            {
                foreach (var item in data.Data)
                {
                    result.Add(item.Data);
                }
            }
            return Ok(new RepositoryResponse<JArray>
            {
                IsSucceed = true,
                Data = result
            });
        }

        // GET api/AttributeSetDatas/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("my-data/{name}")]
        public async Task<ActionResult<List<ODataMobileViewModel>>> ListMyDataByName(string culture, string name, ODataQueryOptions<MixAttributeSetData> queryOptions)
        {
            string _username = User?.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
            Expression<Func<MixAttributeSetData, bool>> predicate = m => m.AttributeSetName == name && m.Specificulture == culture && m.CreatedBy == _username;
            var data = await base.GetListAsync<ODataMobileViewModel>(predicate, queryOptions);
            var result = new JArray();
            if (data != null)
            {
                foreach (var item in data)
                {
                    result.Add(item.Data);
                }
            }
            return Ok(new RepositoryResponse<JArray>
            {
                IsSucceed = true,
                Data = result
            });
        }

        #endregion Get
    }
}