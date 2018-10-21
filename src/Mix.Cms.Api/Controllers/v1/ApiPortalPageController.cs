// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Mix.Domain.Core.ViewModels;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using System.Linq.Expressions;
using System.Web;
using Mix.Cms.Lib.ViewModels.MixPortalPages;
using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Hub;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/permission")]
    public class ApiPortalPageController :
        BaseGenericApiController<MixCmsContext, MixPortalPage>
    {
        public ApiPortalPageController(IMemoryCache memoryCache, IHubContext<PortalHub> hubContext) : base(memoryCache, hubContext)
        {

        }
        #region Get

        // GET api/portalPage/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixPortalPage>> DeleteAsync(int id)
        {
            return await base.DeleteAsync<UpdateViewModel>(
                model => model.Id == id, true);
        }

        // GET api/portalPages/id
        [HttpGet, HttpOptions]
        [Route("details/{id}/{viewType}")]
        [Route("details/{viewType}")]
        public async Task<ActionResult<JObject>> Details(string viewType, int? id)
        {
            string msg = string.Empty;
            switch (viewType)
            {
                case "portal":
                    if (id.HasValue)
                    {
                        Expression<Func<MixPortalPage, bool>> predicate = model => model.Id == id;
                        var portalResult = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixPortalPage()
                        {
                            Status = MixService.GetConfig<int>("DefaultStatus")
                            ,
                            Priority = UpdateViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<UpdateViewModel> result = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
                default:
                    if (id.HasValue)
                    {
                        Expression<Func<MixPortalPage, bool>> predicate = model => model.Id == id;
                        var portalResult = await base.GetSingleAsync<ReadViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixPortalPage()
                        {
                            Status = MixService.GetConfig<int>("DefaultStatus")
                            ,
                            Priority = ReadViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<ReadViewModel> result = await base.GetSingleAsync<ReadViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
            }
        }


        #endregion Get

        #region Post

        // POST api/portalPage
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save([FromBody]UpdateViewModel model)
        {
            if (model != null)
            {
                model.CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
                var result = await base.SaveAsync<UpdateViewModel>(model, true);
                return result;
            }
            return new RepositoryResponse<UpdateViewModel>() { Status = 501 };
        }

        // GET api/portalPage
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            var parsed = HttpUtility.ParseQueryString(request.Query ?? "");
            bool isLevel = int.TryParse(parsed.Get("level"), out int level);
            ParseRequestPagingDate(request);
            Expression<Func<MixPortalPage, bool>> predicate = model =>
                        (!isLevel || model.Level == level)
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (model.TextDefault.Contains(request.Keyword)
                            || model.Description.Contains(request.Keyword)))
                        && (!request.FromDate.HasValue
                            || (model.CreatedDateTime >= request.FromDate.Value)
                        )
                        && (!request.ToDate.HasValue
                            || (model.CreatedDateTime <= request.ToDate.Value)
                        );
            string key = $"{request.Key}_{request.PageSize}_{request.PageIndex}";
            switch (request.Key)
            {
                
                default:
                    var portalResult = await base.GetListAsync<ReadViewModel>(key, request, predicate);
                    return Ok(JObject.FromObject(portalResult));
            }
        }

        // POST api/PortalPage
        [HttpPost, HttpOptions]
        [Route("update-infos")]
        public async Task<RepositoryResponse<List<ReadViewModel>>> UpdateInfos([FromBody]List<ReadViewModel> models)
        {
            if (models != null)
            {
                RemoveCache();
                return await ReadViewModel.UpdateInfosAsync(models);
            }
            else
            {
                return new RepositoryResponse<List<ReadViewModel>>();
            }
        }

        // POST api/PortalPage
        [HttpPost, HttpOptions]
        [Route("update-child-infos")]
        public async Task<RepositoryResponse<List<Lib.ViewModels.MixPortalPagePortalPages.ReadViewModel>>> UpdateNavInfos([FromBody]List<Lib.ViewModels.MixPortalPagePortalPages.ReadViewModel> models)
        {
            if (models != null)
            {
                RemoveCache();
                return await Lib.ViewModels.MixPortalPagePortalPages.ReadViewModel.UpdateInfosAsync(models);
            }
            else
            {
                return new RepositoryResponse<List<Lib.ViewModels.MixPortalPagePortalPages.ReadViewModel>>();
            }
        }

        #endregion Post
    }
}
