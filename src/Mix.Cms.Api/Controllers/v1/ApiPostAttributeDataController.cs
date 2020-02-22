// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixPostAttributeDatas;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/post-attribute-data")]
    public class ApiPostAttributeDataController :
        BaseGenericApiController<MixCmsContext, MixPostAttributeData>
    {
        public ApiPostAttributeDataController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/post-attribute-data/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixPostAttributeData>> DeleteAsync(string id)
        {
            return await base.DeleteAsync<DeleteViewModel>(
                model => model.Id == id && model.Specificulture == _lang, true);
        }

        // GET api/post-attribute-datas/id
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
                        Expression<Func<MixPostAttributeData, bool>> predicate = model => model.Id == id;
                        var portalResult = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixPostAttributeData()
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
                        Expression<Func<MixPostAttributeData, bool>> predicate = model => model.Id == id;
                        var result = await base.GetSingleAsync<ReadViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(result));
                    }
                    else
                    {
                        var model = new MixPostAttributeData()
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

        // POST api/post-attribute-data
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save([FromBody]UpdateViewModel model)
        {
            if (model != null)
            {
                model.Specificulture = _lang;
                var result = await base.SaveAsync<UpdateViewModel>(model, true);
                if (result.IsSucceed)
                {
                    MixService.LoadFromDatabase();
                    MixService.SaveSettings();
                }
                return result;
            }
            return new RepositoryResponse<UpdateViewModel>() { Status = 501 };
        }

        // GET api/post-attribute-data
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            ParseRequestPagingDate(request);
            Expression<Func<MixPostAttributeData, bool>> predicate = model =>
                string.IsNullOrWhiteSpace(request.Keyword)
                    || (model.MixPostAttributeValue.Any(v => v.StringValue.Contains(request.Keyword))
                    );
            switch (request.Key)
            {
                case "portal":
                    var portalResult = await base.GetListAsync<UpdateViewModel>(request, predicate);
                    return Ok(JObject.FromObject(portalResult));

                default:

                    var listItemResult = await base.GetListAsync<ReadViewModel>(request, predicate);

                    return JObject.FromObject(listItemResult);
            }
        }

        #endregion Post
    }
}