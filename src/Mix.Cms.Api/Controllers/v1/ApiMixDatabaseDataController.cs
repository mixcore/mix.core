// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.SignalR.Hubs;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/mix-database-data")]
    public class ApiMixDatabaseDataController :
        BaseApiController<MixCmsContext>
    {
        public ApiMixDatabaseDataController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<PortalHub> hubContext) : base(context, memoryCache, hubContext)
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
            return await Task.FromResult(new JObject());
            //switch (viewType)
            //{
            //    case "portal":
            //        if (!string.IsNullOrEmpty(id))
            //        {
            //            var result = new Lib.ViewModels.MixPostAttributeDatas.UpdateViewModel(setId);
            //            return Ok(JObject.FromObject(result));
            //        }
            //        else
            //        {
            //            var model = new MixPostAttributeData()
            //            {
            //                Status = MixService.GetConfig<int>(MixAppSettingKeywords.DefaultContentStatus),
            //                MixDatabaseId = setId,
            //                Specificulture = _lang,
            //                Priority = Lib.ViewModels.MixPostAttributeDatas.UpdateViewModel.Repository.Max(a => a.Priority).Data + 1
            //            };

            //            RepositoryResponse<Lib.ViewModels.MixPostAttributeDatas.UpdateViewModel> result =
            //                await base.GetSingleAsync<Lib.ViewModels.MixPostAttributeDatas.UpdateViewModel, MixPostAttributeData>($"{viewType}_default", null, model);
            //            return Ok(JObject.FromObject(result));
            //        }
            //    default:
            //        if (!string.IsNullOrEmpty(id))
            //        {
            //            Expression<Func<MixPostAttributeData, bool>> predicate = model => model.Id == id;
            //            var result = await base.GetSingleAsync<Lib.ViewModels.MixPostAttributeDatas.ReadViewModel, MixPostAttributeData>(
            //                $"{viewType}_{id}", predicate);
            //            return Ok(JObject.FromObject(result));
            //        }
            //        else
            //        {
            //            var model = new MixPostAttributeData()
            //            {
            //                Status = MixService.GetConfig<int>(MixAppSettingKeywords.DefaultContentStatus)
            //               ,
            //                Priority = Lib.ViewModels.MixPostAttributeDatas.UpdateViewModel.Repository.Max(a => a.Priority).Data + 1
            //            };

            //            RepositoryResponse<Lib.ViewModels.MixPostAttributeDatas.ReadViewModel> result = await base.GetSingleAsync<Lib.ViewModels.MixPostAttributeDatas.ReadViewModel, MixPostAttributeData>(
            //                $"{viewType}_default", null, model);
            //            return Ok(JObject.FromObject(result));
            //        }
            //}
        }

        #endregion Get
    }
}