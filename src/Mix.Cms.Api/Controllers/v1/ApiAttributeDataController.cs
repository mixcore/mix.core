// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/attribute-data")]
    public class ApiAttributeDataController :
        BaseApiController<MixCmsContext>
    {
        public ApiAttributeDataController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/attribute-datas/id
        [HttpGet, HttpOptions]
        [Route("details/{type}/{setId}/{id}/{viewType}")]
        [Route("details/{type}/{setId}/{viewType}")]
        public async Task<ActionResult<JObject>> Details(int setId, string type, string viewType, string id)
        {
            string msg = string.Empty;
            switch (type)
            {
                case "post":
                    return await GetPostDataAsync(setId, viewType, id);

                case "module":
                    break;

                case "page":
                    break;

                default:
                    break;
            }
            return BadRequest();
        }

        private async Task<ActionResult<JObject>> GetPostDataAsync(int setId, string viewType, string id)
        {
            switch (viewType)
            {
                case "portal":
                    if (!string.IsNullOrEmpty(id))
                    {
                        var result = new Lib.ViewModels.MixPostAttributeDatas.UpdateViewModel(setId);
                        return Ok(JObject.FromObject(result));
                    }
                    else
                    {
                        var model = new MixPostAttributeData()
                        {
                            Status = MixService.GetConfig<int>("DefaultStatus"),
                            AttributeSetId = setId,
                            Specificulture = _lang,
                            Priority = Lib.ViewModels.MixPostAttributeDatas.UpdateViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<Lib.ViewModels.MixPostAttributeDatas.UpdateViewModel> result =
                            await base.GetSingleAsync<Lib.ViewModels.MixPostAttributeDatas.UpdateViewModel, MixPostAttributeData>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
                default:
                    if (!string.IsNullOrEmpty(id))
                    {
                        Expression<Func<MixPostAttributeData, bool>> predicate = model => model.Id == id;
                        var result = await base.GetSingleAsync<Lib.ViewModels.MixPostAttributeDatas.ReadViewModel, MixPostAttributeData>(
                            $"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(result));
                    }
                    else
                    {
                        var model = new MixPostAttributeData()
                        {
                            Status = MixService.GetConfig<int>("DefaultStatus")
                           ,
                            Priority = Lib.ViewModels.MixPostAttributeDatas.UpdateViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<Lib.ViewModels.MixPostAttributeDatas.ReadViewModel> result = await base.GetSingleAsync<Lib.ViewModels.MixPostAttributeDatas.ReadViewModel, MixPostAttributeData>(
                            $"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
            }
        }

        #endregion Get
    }
}