// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.SignalR.Hubs;
using Mix.Cms.Lib.ViewModels.MixConfigurations;
using Mix.Heart.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/configuration")]
    public class ApiConfigurationController :
         BaseGenericApiController<MixCmsContext, MixConfiguration>
    {
        public ApiConfigurationController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<PortalHub> hubContext)
            : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/configuration/keyword
        [HttpGet, HttpOptions]
        [Route("delete/{keyword}")]
        public async Task<RepositoryResponse<MixConfiguration>> DeleteAsync(string keyword)
        {
            var result = await base.DeleteAsync<UpdateViewModel>(
                model => model.Keyword == keyword && model.Specificulture == _lang, true);
            if (result.IsSucceed)
            {
                MixService.SetConfig("LastUpdateConfiguration", DateTime.UtcNow);
            }
            return result;
        }

        // GET api/configurations/keyword
        [HttpGet, HttpOptions]
        [Route("details/{keyword}/{viewType}")]
        [Route("details/{viewType}")]
        public async Task<ActionResult<JObject>> Details(string viewType, string keyword)
        {
            string msg = string.Empty;
            switch (viewType)
            {
                case "portal":
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        Expression<Func<MixConfiguration, bool>> predicate = model => model.Keyword == keyword && model.Specificulture == _lang;
                        var portalResult = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_{keyword}", predicate);
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixConfiguration()
                        {
                            Specificulture = _lang,
                            Category = "Site",
                            Status = MixService.GetAppSetting<MixContentStatus>(MixAppSettingKeywords.DefaultContentStatus),
                            Priority = UpdateViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<UpdateViewModel> result = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
                default:
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        var beResult = await ReadMvcViewModel.Repository.GetSingleModelAsync(model => model.Keyword == keyword && model.Specificulture == _lang).ConfigureAwait(false);
                        return Ok(JObject.FromObject(beResult));
                    }
                    else
                    {
                        var model = new MixConfiguration();
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

        // POST api/configuration
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save([FromBody] UpdateViewModel model)
        {
            var result = await base.SaveAsync<UpdateViewModel>(model, true);
            if (result.IsSucceed)
            {
                MixService.SetConfig("LastUpdateConfiguration", DateTime.UtcNow);
                MixService.LoadFromDatabase();
                MixService.SaveSettings();
            }
            return result;
        }

        // GET api/configuration
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            ParseRequestPagingDate(request);
            Expression<Func<MixConfiguration, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (string.IsNullOrEmpty(request.Status) || model.Status == Enum.Parse<MixContentStatus>(request.Status))
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (model.Keyword.Contains(request.Keyword)
                            || model.Description.Contains(request.Keyword)))
                        && (!request.FromDate.HasValue
                            || (model.CreatedDateTime >= request.FromDate.Value)
                        )
                        && (!request.ToDate.HasValue
                            || (model.CreatedDateTime <= request.ToDate.Value)
                        );
            switch (request.Key)
            {
                case "mvc":
                    var mvcResult = await base.GetListAsync<ReadMvcViewModel>(request, predicate);

                    return Ok(JObject.FromObject(mvcResult));

                case "portal":
                    var portalResult = await base.GetListAsync<UpdateViewModel>(request, predicate);

                    return Ok(JObject.FromObject(portalResult));

                default:

                    var listItemResult = await base.GetListAsync<ReadMvcViewModel>(request, predicate);
                    return JObject.FromObject(listItemResult);
            }
        }

        #endregion Post
    }
}