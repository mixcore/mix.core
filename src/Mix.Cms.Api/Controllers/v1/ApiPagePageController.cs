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
using Mix.Cms.Lib.ViewModels.MixPagePages;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/page-page")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiPagePostController))]
    public class ApiPagePageController :
        BaseGenericApiController<MixCmsContext, MixPagePage>
    {
        public ApiPagePageController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/page/id
        [HttpGet, HttpOptions]
        [Route("delete/{parentId}/{id}")]
        public async Task<RepositoryResponse<MixPagePage>> DeleteAsync(int parentId, int id)
        {
            return await base.DeleteAsync<ReadViewModel>(
                model => model.Id == id && model.ParentId == parentId && model.Specificulture == _lang, true);
        }

        // GET api/pages/id
        [HttpGet, HttpOptions]
        [Route("detail/{parentId}/{id}/{viewType}")]
        public async Task<ActionResult<JObject>> Details(string viewType, int? parentId, int? id)
        {
            string msg = string.Empty;
            switch (viewType)
            {
                default:
                    if (parentId.HasValue && id.HasValue)
                    {
                        Expression<Func<MixPagePage, bool>> predicate = model => model.ParentId == parentId && model.Id == id && model.Specificulture == _lang;
                        var portalResult = await base.GetSingleAsync<ReadViewModel>($"{viewType}_{parentId}_{id}", predicate);
                        if (portalResult.IsSucceed)
                        {
                            portalResult.Data.Page.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                new { action = "post", culture = _lang, portalResult.Data.Page.SeoName }, Request, Url);
                        }

                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixPagePage()
                        {
                            Specificulture = _lang,
                            Status = MixService.GetConfig<int>("DefaultStatus"),
                            Priority = ReadViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<ReadViewModel> result = await base.GetSingleAsync<ReadViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
            }
        }

        #endregion Get

        #region Post

        // POST api/page
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<ReadViewModel>> Save([FromBody]ReadViewModel model)
        {
            if (model != null)
            {
                var result = await base.SaveAsync<ReadViewModel>(model, true);
                return result;
            }
            return new RepositoryResponse<ReadViewModel>() { Status = 501 };
        }

        // POST api/page
        [HttpPost, HttpOptions]
        [Route("save/{parentId}/{id}")]
        public async Task<RepositoryResponse<MixPagePage>> SaveFields(int parentId, int id, [FromBody]List<EntityField> fields)
        {
            if (fields != null)
            {
                var result = new RepositoryResponse<MixPagePage>() { IsSucceed = true };
                foreach (var property in fields)
                {
                    if (result.IsSucceed)
                    {
                        result = await ReadViewModel.Repository.UpdateFieldsAsync(c => c.ParentId == parentId && c.Id == id && c.Specificulture == _lang, fields).ConfigureAwait(false);
                    }
                    else
                    {
                        break;
                    }
                }
                return result;
            }
            return new RepositoryResponse<MixPagePage>();
        }

        // GET api/page
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            var query = HttpUtility.ParseQueryString(request.Query ?? "");
            bool isParent = int.TryParse(query.Get("parent_id"), out int parentId);
            bool isPage = int.TryParse(query.Get("page_id"), out int id);
            ParseRequestPagingDate(request);
            Expression<Func<MixPagePage, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (!isParent || model.ParentId == parentId)
                        && (!isPage || model.ParentId == id)
                        && (!request.Status.HasValue || model.Status == request.Status.Value)
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (model.Description.Contains(request.Keyword)
                            ))
                        ;
            switch (request.Key)
            {
                default:
                    var listItemResult = await base.GetListAsync<ReadViewModel>(request, predicate);
                    listItemResult.Data.Items.ForEach(n => n.IsActived = true);
                    return JObject.FromObject(listItemResult);
            }
        }

        [HttpPost, HttpOptions]
        [Route("update-infos")]
        public async Task<RepositoryResponse<List<ReadViewModel>>> UpdateInfos([FromBody]List<ReadViewModel> models)
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