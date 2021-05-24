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
using Mix.Cms.Lib.ViewModels.MixModulePosts;
using Mix.Heart.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/module-post")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiModulePostController))]
    public class ApiModulePostController :
        BaseGenericApiController<MixCmsContext, MixModulePost>
    {
        public ApiModulePostController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/module/id
        [HttpGet, HttpOptions]
        [Route("delete/{moduleId}/{postId}")]
        public async Task<RepositoryResponse<MixModulePost>> DeleteAsync(int moduleId, int postId)
        {
            return await base.DeleteAsync<ReadViewModel>(
                model => model.PostId == postId && model.ModuleId == moduleId && model.Specificulture == _lang, true);
        }

        // GET api/modules/id
        [HttpGet, HttpOptions]
        [Route("detail/{moduleId}/{postId}/{viewType}")]
        public async Task<ActionResult<JObject>> Details(string viewType, int? moduleId, int? postId)
        {
            string msg = string.Empty;
            switch (viewType)
            {
                default:
                    if (moduleId.HasValue && postId.HasValue)
                    {
                        Expression<Func<MixModulePost, bool>> predicate = model => model.ModuleId == moduleId && model.PostId == postId && model.Specificulture == _lang;
                        var portalResult = await base.GetSingleAsync<ReadViewModel>($"{viewType}_{moduleId}_{postId}", predicate);
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixModulePost()
                        {
                            Specificulture = _lang,
                            Status = MixService.GetEnumConfig<MixContentStatus>(MixAppSettingKeywords.DefaultContentStatus),
                            Priority = ReadViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<ReadViewModel> result = await base.GetSingleAsync<ReadViewModel>($"{viewType}_default", null, model);
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
        public async Task<RepositoryResponse<ReadViewModel>> Save([FromBody] ReadViewModel model)
        {
            if (model != null)
            {
                var result = await base.SaveAsync<ReadViewModel>(model, true);
                return result;
            }
            return new RepositoryResponse<ReadViewModel>() { Status = 501 };
        }

        // POST api/module
        [HttpPost, HttpOptions]
        [Route("save/{id}/{postId}")]
        public async Task<RepositoryResponse<MixModulePost>> SaveFields(int moduleId, int postId, [FromBody] List<EntityField> fields)
        {
            if (fields != null)
            {
                var result = new RepositoryResponse<MixModulePost>() { IsSucceed = true };
                foreach (var property in fields)
                {
                    if (result.IsSucceed)
                    {
                        result = await ReadViewModel.Repository.UpdateFieldsAsync(c => c.ModuleId == moduleId && c.PostId == postId && c.Specificulture == _lang, fields).ConfigureAwait(false);
                    }
                    else
                    {
                        break;
                    }
                }
                return result;
            }
            return new RepositoryResponse<MixModulePost>();
        }

        // GET api/module
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            var query = HttpUtility.ParseQueryString(request.Query ?? "");
            bool isModule = int.TryParse(query.Get("module_id"), out int moduleId);
            bool isPost = int.TryParse(query.Get("post_id"), out int postId);
            ParseRequestPagingDate(request);
            Expression<Func<MixModulePost, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (!isModule || model.ModuleId == moduleId)
                        && (!isPost || model.PostId == postId)
                        && (string.IsNullOrEmpty(request.Status) || model.Status == Enum.Parse<MixContentStatus>(request.Status))
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (model.Description.Contains(request.Keyword)
                            ))
                        ;
            string key = $"{request.Key}_{request.Query}_{request.PageSize}_{request.PageIndex}";
            switch (request.Key)
            {
                default:

                    var listItemResult = await base.GetListAsync<ReadViewModel>(request, predicate);
                    return JObject.FromObject(listItemResult);
            }
        }

        // POST api/update-infos
        [HttpPost, HttpOptions]
        [Route("update-infos")]
        public async Task<RepositoryResponse<List<ReadViewModel>>> UpdateInfos([FromBody] List<ReadViewModel> models)
        {
            if (models != null)
            {
                return await base.SaveListAsync(models, false);
            }
            else
            {
                return new RepositoryResponse<List<ReadViewModel>>();
            }
        }

        // POST api/update-infos
        [HttpPost, HttpOptions]
        [Route("save-list")]
        public async Task<RepositoryResponse<List<ReadViewModel>>> SaveList([FromBody] List<ReadViewModel> models)
        {
            if (models != null)
            {
                return await base.SaveListAsync(models, false);
            }
            else
            {
                return new RepositoryResponse<List<ReadViewModel>>();
            }
        }

        #endregion Post
    }
}