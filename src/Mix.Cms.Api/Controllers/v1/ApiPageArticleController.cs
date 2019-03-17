// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mix.Domain.Core.ViewModels;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using System.Linq.Expressions;
using System.Web;
using Mix.Cms.Lib.ViewModels.MixPageArticles;
using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Hub;
using Microsoft.Extensions.Caching.Memory;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/page-article")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiPageArticleController))]
    public class ApiPageArticleController :
        BaseGenericApiController<MixCmsContext, MixPageArticle>
    {
        public ApiPageArticleController(IMemoryCache memoryCache, IHubContext<PortalHub> hubContext) : base(memoryCache, hubContext)
        {

        }
        #region Get

        // GET api/page/id
        [HttpGet, HttpOptions]
        [Route("delete/{pageId}/{articleId}")]
        public async Task<RepositoryResponse<MixPageArticle>> DeleteAsync(int pageId, int articleId)
        {
            return await base.DeleteAsync<ReadViewModel>(
                model => model.ArticleId == articleId && model.CategoryId == pageId && model.Specificulture == _lang, true);
        }

        // GET api/pages/id
        [HttpGet, HttpOptions]
        [Route("detail/{pageId}/{articleId}/{viewType}")]
        public async Task<ActionResult<JObject>> Details(string viewType, int? pageId, int? articleId)
        {
            string msg = string.Empty;
            switch (viewType)
            {

                default:
                    if (pageId.HasValue && articleId.HasValue)
                    {
                        Expression<Func<MixPageArticle, bool>> predicate = model => model.CategoryId == pageId && model.ArticleId == articleId && model.Specificulture == _lang;
                        var portalResult = await base.GetSingleAsync<ReadViewModel>($"{viewType}_{pageId}_{articleId}", predicate);
                        if (portalResult.IsSucceed)
                        {
                            portalResult.Data.Article.DetailsUrl = MixCmsHelper.GetRouterUrl("Article", new { portalResult.Data.Article.SeoName }, Request, Url);
                        }

                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixPageArticle()
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
        [Route("save/{id}/{articleId}")]
        public async Task<RepositoryResponse<MixPageArticle>> SaveFields(int pageId, int articleId, [FromBody]List<EntityField> fields)
        {
            if (fields != null)
            {
                var result = new RepositoryResponse<MixPageArticle>() { IsSucceed = true };
                foreach (var property in fields)
                {
                    if (result.IsSucceed)
                    {
                        result = await ReadViewModel.Repository.UpdateFieldsAsync(c => c.CategoryId == pageId && c.ArticleId == articleId && c.Specificulture == _lang, fields).ConfigureAwait(false);
                    }
                    else
                    {
                        break;
                    }

                }
                return result;
            }
            return new RepositoryResponse<MixPageArticle>();
        }

        // GET api/page
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            var query = HttpUtility.ParseQueryString(request.Query ?? "");
            bool isPage = int.TryParse(query.Get("page_id"), out int pageId);            
            bool isArticle = int.TryParse(query.Get("article_id"), out int articleId);
            ParseRequestPagingDate(request);
            Expression<Func<MixPageArticle, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (!isPage || model.CategoryId == pageId)                        
                        && (!isArticle || model.ArticleId == articleId)
                        && (!request.Status.HasValue || model.Status == request.Status.Value)
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (model.Description.Contains(request.Keyword)
                            ))
                        ;
            string key = $"{request.Key}_{request.Query}_{request.PageSize}_{request.PageIndex}";
            switch (request.Key)
            {
                default:
                    var listItemResult = await base.GetListAsync<ReadViewModel>(key, request, predicate);
                    listItemResult.Data.Items.ForEach(n => n.IsActived = true);
                    listItemResult.Data.Items.ForEach(n => n.Article.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                "article",  new { id = n.Article.Id, seoName = n.Article.SeoName }, Request, Url));
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