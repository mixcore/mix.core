// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Lib.ViewModels.MixPosts;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/post")]
    public class ApiPostController :
        BaseGenericApiController<MixCmsContext, MixPost>
    {
        public ApiPostController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/post/id

        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixPost>> DeleteAsync(int id)
        {
            return await base.DeleteAsync<RemoveViewModel>(
                model => model.Id == id && model.Specificulture == _lang, true);
        }

        // GET api/posts/id
        [AllowAnonymous]
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
                        Expression<Func<MixPost, bool>> predicate = model => model.Id == id && model.Specificulture == _lang;
                        var portalResult = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_{id}", predicate);
                        if (portalResult.IsSucceed)
                        {
                            portalResult.Data.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                new { action = "post", culture = _lang, id = portalResult.Data.Id, SeoName = portalResult.Data.SeoName }, Request, Url);
                        }

                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixPost()
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
                        if (beResult.IsSucceed)
                        {
                            beResult.Data.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                new { action = "post", culture = _lang, id = beResult.Data.Id, beResult.Data.SeoName }, Request, Url);
                        }
                        return Ok(JObject.FromObject(beResult));
                    }
                    else
                    {
                        var model = new MixPost();
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

        // POST api/post
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save([FromBody]UpdateViewModel model)
        {
            if (model != null)
            {
                model.CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
                var result = await base.SaveAsync<UpdateViewModel>(model, true);
                if (result.IsSucceed)
                {
                    if (model.Status == MixEnums.MixContentStatus.Schedule)
                    {
                        DateTime dtPublish = DateTime.UtcNow;
                        if (model.PublishedDateTime.HasValue)
                        {
                            dtPublish = model.PublishedDateTime.Value;
                        }
                        MixService.SetConfig(MixConstants.ConfigurationKeyword.NextSyncContent, dtPublish);
                        MixService.SaveSettings();
                        MixService.Reload();
                    }
                }
                return result;
            }
            return new RepositoryResponse<UpdateViewModel>() { Status = 501 };
        }

        [HttpPost, HttpOptions]
        [Route("save-list")]
        public async Task<RepositoryResponse<List<SyncViewModel>>> SaveList([FromBody]List<SyncViewModel> models)
        {
            if (models != null)
            {
                return await base.SaveListAsync(models, true);
            }
            else
            {
                return new RepositoryResponse<List<SyncViewModel>>();
            }
        }

        // POST api/post
        [HttpPost, HttpOptions]
        [Route("save/{id}")]
        public async Task<RepositoryResponse<MixPost>> SaveFields(int id, [FromBody]List<EntityField> fields)
        {
            if (fields != null)
            {
                var result = new RepositoryResponse<MixPost>() { IsSucceed = true };
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
            return new RepositoryResponse<MixPost>();
        }

        // GET api/post
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            var query = HttpUtility.ParseQueryString(request.Query ?? "");
            bool isPage = int.TryParse(query.Get("page_id"), out int pageId);
            bool isNotPage = int.TryParse(query.Get("not_page_id"), out int notPageId);
            bool isModule = int.TryParse(query.Get("module_id"), out int moduleId);
            bool isNotModule = int.TryParse(query.Get("not_module_id"), out int notModuleId);
            ParseRequestPagingDate(request);
            Expression<Func<MixPost, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (!request.Status.HasValue || model.Status == request.Status.Value)
                        && (!isPage || model.MixPagePost.Any(nav => nav.PageId == pageId && nav.PostId == model.Id && nav.Specificulture == _lang))
                        && (!isNotPage || !model.MixPagePost.Any(nav => nav.PageId == notPageId && nav.PostId == model.Id && nav.Specificulture == _lang))
                        && (!isModule || model.MixModulePost.Any(nav => nav.ModuleId == moduleId && nav.PostId == model.Id))
                        && (!isNotModule || !model.MixModulePost.Any(nav => nav.ModuleId == notModuleId && nav.PostId == model.Id))
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (model.Title.Contains(request.Keyword)
                            || model.Excerpt.Contains(request.Keyword)))
                        && (!request.FromDate.HasValue
                            || (model.CreatedDateTime >= request.FromDate.Value)
                        )
                        && (!request.ToDate.HasValue
                            || (model.CreatedDateTime <= request.ToDate.Value)
                        );

            var nextSync = PublishPosts();

            switch (request.Key)
            {
                case "service.store":
                    var srvResult = await base.GetListAsync<Lib.ViewModels.Services.Store.PostViewModel>(request, predicate);
                    if (srvResult.IsSucceed)
                    {
                        srvResult.Data.Items.ForEach(a =>
                        {
                            a.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                new { action = "post", culture = _lang, id = a.Id, seoName = a.SeoName }, Request, Url);
                        });
                    }
                    return Ok(JObject.FromObject(srvResult));

                case "mvc":
                    var mvcResult = await base.GetListAsync<ReadMvcViewModel>(request, predicate);
                    if (mvcResult.IsSucceed)
                    {
                        mvcResult.Data.Items.ForEach(a =>
                        {
                            a.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                new { action = "post", culture = _lang, id = a.Id, seoName = a.SeoName }, Request, Url);
                        });
                    }
                    return Ok(JObject.FromObject(mvcResult));

                case "portal":
                    var portalResult = await base.GetListAsync<UpdateViewModel>(request, predicate);
                    if (portalResult.IsSucceed)
                    {
                        portalResult.Data.Items.ForEach(a =>
                        {
                            a.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                new { action = "post", culture = _lang, id = a.Id, seoName = a.SeoName }, Request, Url);
                        });
                    }
                    return Ok(JObject.FromObject(portalResult));

                default:

                    var listItemResult = await base.GetListAsync<ReadListItemViewModel>(request, predicate);
                    if (listItemResult.IsSucceed)
                    {
                        listItemResult.Data.Items.ForEach((Action<ReadListItemViewModel>)(a =>
                        {
                            a.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                new { action = "post", culture = _lang, id = a.Id, seoName = a.SeoName }, Request, Url);
                        }));
                    }

                    return JObject.FromObject(listItemResult);
            }
        }

        // POST api/update-infos
        [HttpPost, HttpOptions]
        [Route("update-infos")]
        public async Task<RepositoryResponse<List<ReadListItemViewModel>>> UpdateInfos([FromBody]List<ReadListItemViewModel> models)
        {
            if (models != null)
            {
                return await base.SaveListAsync(models, false);
            }
            else
            {
                return new RepositoryResponse<List<ReadListItemViewModel>>();
            }
        }

        [HttpPost, HttpOptions]
        [Route("apply-list")]
        public async Task<ActionResult<JObject>> ListActionAsync([FromBody]ListAction<int> data)
        {
            Expression<Func<MixPost, bool>> predicate = model =>
                       model.Specificulture == _lang
                       && data.Data.Contains(model.Id);
            var result = new RepositoryResponse<bool>();
            switch (data.Action)
            {
                case "Delete":
                    return Ok(JObject.FromObject(await base.DeleteListAsync<RemoveViewModel>(predicate, true)));

                case "Export":
                    return Ok(JObject.FromObject(await base.ExportListAsync(predicate, MixStructureType.Module)));

                default:
                    return JObject.FromObject(new RepositoryResponse<bool>());
            }
        }

        #endregion Post

        private DateTime? PublishPosts()
        {
            var nextSync = MixService.GetConfig<DateTime?>(MixConstants.ConfigurationKeyword.NextSyncContent);
            if (nextSync.HasValue && nextSync.Value <= DateTime.UtcNow)
            {
                var publishedPosts = ReadListItemViewModel.Repository.GetModelListBy(
                    a => a.Status == (int)MixContentStatus.Schedule
                        && (!a.PublishedDateTime.HasValue || a.PublishedDateTime.Value <= DateTime.UtcNow)
                        );
                publishedPosts.Data.ForEach(a => a.Status = MixContentStatus.Published);
                base.SaveList(publishedPosts.Data, false);
                var next = ReadListItemViewModel.Repository.Min(a => a.Type == (int)MixContentStatus.Schedule,
                            a => a.PublishedDateTime);
                nextSync = next.Data;
                MixService.SetConfig(MixConstants.ConfigurationKeyword.NextSyncContent, nextSync);
                MixService.SaveSettings();
                MixService.Reload();
                return nextSync;
            }
            else
            {
                return nextSync;
            }
        }
    }
}