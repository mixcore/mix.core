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
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using static Mix.Cms.Lib.MixEnums;
using System.Linq.Expressions;
using Mix.Cms.Lib.ViewModels.MixArticles;

namespace Mix.Cms.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/article")]
    public class ApiArticleController :
        BaseApiController
    {
        public ApiArticleController()
        {
        }

        #region Get

        // GET api/category/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixArticle>> DeleteAsync(int id)
        {
            var getArticle = await ReadListItemViewModel.Repository.GetSingleModelAsync(
                model => model.Id == id && model.Specificulture == _lang);
            if (getArticle.IsSucceed)
            {

                return await getArticle.Data.RemoveModelAsync(true);
            }
            else
            {
                return new RepositoryResponse<MixArticle>()
                {
                    IsSucceed = false
                };
            }
        }

        // GET api/articles/id
        [HttpGet, HttpOptions]
        [Route("details/{id}/{viewType}")]
        [Route("details/{viewType}")]
        public async Task<ActionResult<JObject>> Details(string viewType, int? id)
        {
            switch (viewType)
            {
                case "portal":
                    if (id.HasValue)
                    {
                        var beResult = await UpdateViewModel.Repository.GetSingleModelAsync(model => model.Id == id && model.Specificulture == _lang).ConfigureAwait(false);
                        if (beResult.IsSucceed)
                        {
                            beResult.Data.DetailsUrl = MixCmsHelper.GetRouterUrl("Article", new { beResult.Data.SeoName }, Request, Url);
                        }
                        return Ok(JObject.FromObject(beResult));
                    }
                    else
                    {
                        var model = new MixArticle()
                        {
                            Specificulture = _lang,
                            Status = MixService.GetConfig<int>("DefaultStatus"),
                            Priority = UpdateViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<UpdateViewModel> result = new RepositoryResponse<UpdateViewModel>()
                        {
                            IsSucceed = true,
                            Data = await UpdateViewModel.InitViewAsync(model)
                        };
                        return JObject.FromObject(result);
                    }
                default:
                    if (id.HasValue)
                    {
                        var beResult = await UpdateViewModel.Repository.GetSingleModelAsync(model => model.Id == id && model.Specificulture == _lang).ConfigureAwait(false);
                        if (beResult.IsSucceed)
                        {
                            beResult.Data.DetailsUrl = MixCmsHelper.GetRouterUrl("Article", new { beResult.Data.SeoName }, Request, Url);
                        }
                        return JObject.FromObject(beResult);
                    }
                    else
                    {
                        var model = new MixArticle();
                        RepositoryResponse<UpdateViewModel> result = new RepositoryResponse<UpdateViewModel>()
                        {
                            IsSucceed = true,
                            Data = new UpdateViewModel(model)
                            {
                                Specificulture = _lang,
                                Status = MixContentStatus.Preview,
                            }
                        };
                        return JObject.FromObject(result);
                    }
            }
        }


        #endregion Get

        #region Post

        // POST api/category
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Post([FromBody]UpdateViewModel model)
        {
            if (model != null)
            {
                model.CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
                var result = await model.SaveModelAsync(true).ConfigureAwait(false);
                return result;
            }
            return new RepositoryResponse<UpdateViewModel>() { Status = 501 };
        }

        // POST api/category
        [HttpPost, HttpOptions]
        [Route("save/{id}")]
        public async Task<RepositoryResponse<MixArticle>> SaveFields(int id, [FromBody]List<EntityField> fields)
        {
            if (fields != null)
            {
                var result = new RepositoryResponse<MixArticle>() { IsSucceed = true };
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
            return new RepositoryResponse<MixArticle>();
        }
        // GET api/category
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<JObject> GetList(
            [FromBody] RequestPaging request)
        {
            ParseRequestPagingDate(request);
            Expression<Func<MixArticle, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (!request.Status.HasValue || model.Status == request.Status.Value)
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (model.Title.Contains(request.Keyword)
                            || model.Excerpt.Contains(request.Keyword)))
                        && (!request.FromDate.HasValue
                            || (model.CreatedDateTime >= request.FromDate.Value)
                        )
                        && (!request.ToDate.HasValue
                            || (model.CreatedDateTime <= request.ToDate.Value)
                        );

            switch (request.Key)
            {
                case "mvc":
                    var fedata = await ReadMvcViewModel.Repository.GetModelListByAsync
                        (predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);
                    if (fedata.IsSucceed)
                    {
                        fedata.Data.Items.ForEach(a =>
                        {
                            a.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                "Article", new { seoName = a.SeoName }, Request, Url);
                        });
                    }
                    return JObject.FromObject(fedata);
                case "portal":

                    var bedata = await UpdateViewModel.Repository.GetModelListByAsync
                        (predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);
                    if (bedata.IsSucceed)
                    {
                        bedata.Data.Items.ForEach(a =>
                        {
                            a.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                "Article", new { seoName = a.SeoName }, Request, Url);
                        });
                    }
                    return JObject.FromObject(bedata);
                default:

                    var data = await ReadListItemViewModel.Repository.GetModelListByAsync
                        (predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);
                    if (data.IsSucceed)
                    {
                        data.Data.Items.ForEach((Action<ReadListItemViewModel>)(a =>
                        {
                            a.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                "Article", new { seoName = a.SeoName }, Request, Url);
                        }));
                    }
                    return JObject.FromObject(data);
            }
        }

        #endregion Post
    }
}