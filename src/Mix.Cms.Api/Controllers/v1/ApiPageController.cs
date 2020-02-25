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
using Mix.Cms.Lib.ViewModels.MixPages;
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
    [Route("api/v1/{culture}/page")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiPageController))]
    public class ApiPageController :
        BaseGenericApiController<MixCmsContext, MixPage>
    {
        public ApiPageController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/page/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixPage>> DeleteAsync(int id)
        {
            return await base.DeleteAsync<UpdateViewModel>(
                model => model.Id == id && model.Specificulture == _lang, true);
        }

        // GET api/pages/id
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
                        Expression<Func<MixPage, bool>> predicate = model => model.Id == id && model.Specificulture == _lang;
                        var portalResult = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_{id}", predicate);
                        if (portalResult.IsSucceed)
                        {
                            portalResult.Data.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                new { culture = _lang, seoName = portalResult.Data.SeoName }, Request, Url);
                        }

                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixPage()
                        {
                            Specificulture = _lang,
                            Status = MixService.GetConfig<int>("DefaultStatus"),
                            PageSize = 20
                            ,
                            Priority = UpdateViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<UpdateViewModel> result = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
                case "listItem":
                    if (id.HasValue)
                    {
                        var beResult = await ReadListItemViewModel.Repository.GetSingleModelAsync(model => model.Id == id && model.Specificulture == _lang).ConfigureAwait(false);
                        if (beResult.IsSucceed)
                        {
                            beResult.Data.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                new { culture = _lang, beResult.Data.SeoName }, Request, Url);
                        }
                        return Ok(JObject.FromObject(beResult));
                    }
                    else
                    {
                        var model = new MixPage();
                        RepositoryResponse<ReadListItemViewModel> result = new RepositoryResponse<ReadListItemViewModel>()
                        {
                            IsSucceed = true,
                            Data = new ReadListItemViewModel(model)
                            {
                                Specificulture = _lang,
                                Status = MixEnums.PageStatus.Preview,
                                PageSize = 20
                            }
                        };
                        return Ok(JObject.FromObject(result));
                    }
                default:
                    if (id.HasValue)
                    {
                        var beResult = await ReadMvcViewModel.Repository.GetSingleModelAsync(model => model.Id == id && model.Specificulture == _lang).ConfigureAwait(false);
                        if (beResult.IsSucceed)
                        {
                            beResult.Data.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                new { culture = _lang, seoName = beResult.Data.SeoName }, Request, Url);
                        }
                        return Ok(JObject.FromObject(beResult));
                    }
                    else
                    {
                        var model = new MixPage();
                        RepositoryResponse<ReadMvcViewModel> result = new RepositoryResponse<ReadMvcViewModel>()
                        {
                            IsSucceed = true,
                            Data = new ReadMvcViewModel(model)
                            {
                                Specificulture = _lang,
                                Status = MixContentStatus.Preview,
                                PageSize = 20
                            }
                        };
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

        // POST api/page
        [HttpPost, HttpOptions]
        [Route("save/{id}")]
        public async Task<RepositoryResponse<MixPage>> SaveFields(int id, [FromBody]List<EntityField> fields)
        {
            if (fields != null)
            {
                var result = new RepositoryResponse<MixPage>() { IsSucceed = true };
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
            return new RepositoryResponse<MixPage>();
        }

        // GET api/page
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            var parsed = HttpUtility.ParseQueryString(request.Query ?? "");
            bool isLevel = int.TryParse(parsed.Get("level"), out int level);
            bool isType = int.TryParse(parsed.Get("pageType"), out int pageType);
            ParseRequestPagingDate(request);
            Expression<Func<MixPage, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (!request.Status.HasValue || model.Status == request.Status.Value)
                        && (!isLevel || model.Level == level)
                        && (!isType || model.Type == pageType)
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
                    var mvcResult = await base.GetListAsync<ReadMvcViewModel>(request, predicate);
                    if (mvcResult.IsSucceed)
                    {
                        mvcResult.Data.Items.ForEach(a =>
                        {
                            a.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                new { action = "page", culture = _lang, seoName = a.SeoName }, Request, Url);
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
                                new { action = "page", culture = _lang, seoName = a.SeoName }, Request, Url);
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
                                new { action = "page", culture = _lang, seoName = a.SeoName }, Request, Url);
                            a.Childs.ForEach((Action<Lib.ViewModels.MixPagePages.ReadViewModel>)(c =>
                            {
                                c.Page.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                    new { action = "page", culture = _lang, seoName = c.Page.SeoName }, Request, Url);
                            }));
                        }));
                    }

                    return JObject.FromObject(listItemResult);
            }
        }

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

        #endregion Post
    }
}