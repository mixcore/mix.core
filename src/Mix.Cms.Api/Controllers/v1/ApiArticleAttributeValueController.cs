// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Mix.Domain.Core.ViewModels;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using System.Linq.Expressions;
using Mix.Cms.Lib.ViewModels.MixArticleAttributeValues;
using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Hub;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Attributes;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/article-attribute-value")]
    public class ApiArticleAttributeValueController :
        BaseGenericApiController<MixCmsContext, MixArticleAttributeValue>
    {
        public ApiArticleAttributeValueController(IMemoryCache memoryCache, IHubContext<PortalHub> hubContext) : base(memoryCache, hubContext)
        {

        }
        #region Get

        // GET api/article-attribute-value/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixArticleAttributeValue>> DeleteAsync(string id)
        {
            return await base.DeleteAsync<DeleteViewModel>(
                model => model.Id == id, true);
        }

        // GET api/article-attribute-values/id
        [HttpGet, HttpOptions]
        [Route("details/{id}/{viewType}")]
        [Route("details/{viewType}")]
        public async Task<ActionResult<JObject>> Details(string viewType, string id)
        {
            string msg = string.Empty;
            switch (viewType)
            {
                case "portal":
                    if (!string.IsNullOrEmpty(id))
                    {
                        Expression<Func<MixArticleAttributeValue, bool>> predicate = model => model.Id == id;
                        var portalResult = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixArticleAttributeValue()
                        {
                            Status = MixService.GetConfig<int>("DefaultStatus")
                            ,
                            Priority = UpdateViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<UpdateViewModel> result = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
                default:
                    if (!string.IsNullOrEmpty(id))
                    {
                        Expression<Func<MixArticleAttributeValue, bool>> predicate = model => model.Id == id;
                        var result = await base.GetSingleAsync<ReadViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(result));
                    }
                    else
                    {
                        var model = new MixArticleAttributeValue()
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

        // POST api/article-attribute-value
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]        
        [HttpPost, HttpOptions]
        [RequestFormSizeLimit(valueCountLimit: 214748364)] // 200Mb
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save(UpdateViewModel data)
        {
            if (data != null)
            {
                data.Specificulture = _lang;
                var result = await base.SaveAsync<UpdateViewModel>(data, true);
                if (result.IsSucceed)
                {
                    MixService.LoadFromDatabase();
                    MixService.SaveSettings();
                }
                return result;
            }
            return new RepositoryResponse<UpdateViewModel>() { Status = 501 };
        }

        // GET api/article-attribute-value
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {

            ParseRequestPagingDate(request);
            Expression<Func<MixArticleAttributeValue, bool>> predicate = model =>
                string.IsNullOrWhiteSpace(request.Keyword)
                    || (model.AttributeName.Contains(request.Keyword)
                    );
            string key = $"{request.Key}_{request.PageSize}_{request.PageIndex}";
            switch (request.Key)
            {
                case "portal":
                    var portalResult = await base.GetListAsync<UpdateViewModel>(key, request, predicate);
                    return Ok(JObject.FromObject(portalResult));
                default:

                    var listItemResult = await base.GetListAsync<ReadViewModel>(key, request, predicate);

                    return JObject.FromObject(listItemResult);
            }
        }

        #endregion Post
    }
}
