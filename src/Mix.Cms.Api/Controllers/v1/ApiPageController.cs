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
using System.Web;
using Mix.Cms.Lib.ViewModels.MixPages;
using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Hub;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;

namespace Mix.Cms.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/page")]
    public class ApiPageController :
        BaseGenericApiControoler<MixCmsContext, MixPage>
    {
        public ApiPageController(IMemoryCache memoryCache, IHubContext<PortalHub> hubContext) : base(memoryCache, hubContext)
        {

        }
        #region Get

        // GET api/category/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixPage>> DeleteAsync(int id)
        {
            var getPage = await ReadListItemViewModel.Repository.GetSingleModelAsync(
                model => model.Id == id && model.Specificulture == _lang);
            if (getPage.IsSucceed)
            {

                return await getPage.Data.RemoveModelAsync(true);
            }
            else
            {
                return new RepositoryResponse<MixPage>()
                {
                    IsSucceed = false
                };
            }
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
                            portalResult.Data.DetailsUrl = MixCmsHelper.GetRouterUrl("Page", new { portalResult.Data.SeoName }, Request, Url);
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
                        return JObject.FromObject(result);
                    }
                default:
                    if (id.HasValue)
                    {
                        var beResult = await ReadMvcViewModel.Repository.GetSingleModelAsync(model => model.Id == id && model.Specificulture == _lang).ConfigureAwait(false);
                        if (beResult.IsSucceed)
                        {
                            beResult.Data.DetailsUrl = MixCmsHelper.GetRouterUrl("Page", new { beResult.Data.SeoName }, Request, Url);
                        }
                        return JObject.FromObject(beResult);
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
                        return JObject.FromObject(result);
                    }
            }
        }


        #endregion Get

        #region Post
        [HttpPost, HttpOptions]
        [Route("encrypted/save")]
        public async Task<JObject> Post([FromBody] RequestEncrypted request)
        {
            var key = Convert.FromBase64String(request.Key); //Encoding.UTF8.GetBytes(request.Key);
            var iv = Convert.FromBase64String(request.IV); //Encoding.UTF8.GetBytes(request.IV);
            string encrypted = string.Empty;
            string decrypt = string.Empty;
            if (!string.IsNullOrEmpty(request.PlainText))
            {
                encrypted = MixService.EncryptStringToBytes_Aes(request.PlainText, key, iv).ToString();
            }
            if (!string.IsNullOrEmpty(request.Encrypted))
            {
                decrypt = MixService.DecryptStringFromBytes_Aes(request.Encrypted, key, iv);
            }
            JObject data = new JObject(
                new JProperty("key", request.Key), 
                new JProperty("encrypted", encrypted), 
                new JProperty("plainText", decrypt));
            
            return data;
        }
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

        // GET api/category
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<JObject> GetList(
            [FromBody] RequestPaging request)
        {
            var parsed = HttpUtility.ParseQueryString(request.Query ?? "");
            bool isLevel = int.TryParse(parsed.Get("level"), out int level);
            ParseRequestPagingDate(request);
            Expression<Func<MixPage, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (!request.Status.HasValue || model.Status == request.Status.Value)
                        && (!isLevel || model.Level == level)
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
                    var fedata = await ReadMvcViewModel.Repository.GetModelListByAsync(predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);
                    if (fedata.IsSucceed)
                    {
                        fedata.Data.Items.ForEach(a =>
                        {
                            a.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                "page", new { seoName = a.SeoName }, Request, Url);
                        });
                    }

                    AlertAsync("Get List Page", 200, $"Get {request.Key} list page");
                    return JObject.FromObject(fedata);
                case "portal":

                    var bedata = await UpdateViewModel.Repository.GetModelListByAsync(predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);
                    if (bedata.IsSucceed)
                    {
                        bedata.Data.Items.ForEach(a =>
                        {
                            a.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                "page", new { seoName = a.SeoName }, Request, Url);
                        });
                    }

                    AlertAsync("Get List Page", 200, $"Get {request.Key} list page");
                    return JObject.FromObject(bedata);
                default:

                    var data = await ReadListItemViewModel.Repository.GetModelListByAsync(predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);
                    if (data.IsSucceed)
                    {
                        data.Data.Items.ForEach((Action<ReadListItemViewModel>)(a =>
                        {
                            a.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                "page", new { seoName = a.SeoName }, Request, Url);
                            a.Childs.ForEach((Action<ReadListItemViewModel>)(c =>
                            {
                                c.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                    "page", new { seoName = c.SeoName }, Request, Url);
                            }));
                        }));
                    }

                    AlertAsync("Get List Page", 200, $"Get {request.Key} list page");
                    return JObject.FromObject(data);
            }
        }

        #endregion Post
    }
}