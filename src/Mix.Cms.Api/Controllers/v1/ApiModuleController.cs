// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mix.Domain.Core.ViewModels;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using static Mix.Cms.Lib.MixEnums;
using System.Linq.Expressions;
using System.Web;
using Mix.Cms.Lib.ViewModels.MixModules;
using Microsoft.Extensions.Caching.Memory;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/module")]
    public class ApiModuleController :
         BaseGenericApiController<MixCmsContext, MixModule>
    {
        public ApiModuleController(IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/module/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixModule>> DeleteAsync(int id)
        {
            return await base.DeleteAsync<UpdateViewModel>(
                model => model.Id == id && model.Specificulture == _lang, true);
        }

        // GET api/modules/id
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
                        Expression<Func<MixModule, bool>> predicate = model => model.Id == id && model.Specificulture == _lang;
                        var portalResult = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixModule()
                        {
                            Specificulture = _lang,
                            Status = MixService.GetConfig<int>("DefaultStatus"),
                            Priority = UpdateViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<UpdateViewModel> result = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
                default:
                    if (id.HasValue)
                    {
                        var beResult = await ReadMvcViewModel.Repository.GetSingleModelAsync(model => model.Id == id && model.Specificulture == _lang).ConfigureAwait(false);
                        return Ok(JObject.FromObject(beResult));
                    }
                    else
                    {
                        var model = new MixModule();
                        RepositoryResponse<ReadMvcViewModel> result = new RepositoryResponse<ReadMvcViewModel>()
                        {
                            IsSucceed = true,
                            Data = new ReadMvcViewModel(model)
                            {
                                Specificulture = _lang,
                                Status = MixContentStatus.Preview,
                            }
                        };
                        return Ok(JObject.FromObject(result));
                    }
            }
        }


        #endregion Get

        #region Post

        // POST api/module
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

        // POST api/module
        [HttpPost, HttpOptions]
        [Route("save/{id}")]
        public async Task<RepositoryResponse<MixModule>> SaveFields(int id, [FromBody]List<EntityField> fields)
        {
            if (fields != null)
            {
                var result = new RepositoryResponse<MixModule>() { IsSucceed = true };
                foreach (var property in fields)
                {
                    if (result.IsSucceed)
                    {
                        result = await ReadListItemViewModel.Repository.UpdateFieldsAsync(c => c.Id == id && c.Specificulture == _lang, fields).ConfigureAwait(false);
                    }
                    else
                    {
                        break;
                    }

                }
                return result;
            }
            return new RepositoryResponse<MixModule>();
        }

        // GET api/module
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            var parsed = HttpUtility.ParseQueryString(request.Query ?? "");
            bool isLevel = int.TryParse(parsed.Get("level"), out int level);
            ParseRequestPagingDate(request);
            Expression<Func<MixModule, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (!request.Status.HasValue || model.Status == request.Status.Value)
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (model.Title.Contains(request.Keyword)
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
                case "mvc":
                    var mvcResult = await base.GetListAsync<ReadMvcViewModel>(key, request, predicate);
                 
                    return Ok(JObject.FromObject(mvcResult));
                case "portal":
                    var portalResult = await base.GetListAsync<UpdateViewModel>(key, request, predicate);
                   
                    return Ok(JObject.FromObject(portalResult));
                default:

                    var listItemResult = await base.GetListAsync<ReadListItemViewModel>(key, request, predicate);
                    return JObject.FromObject(listItemResult);
            }
        }

        #endregion Post
    }
}