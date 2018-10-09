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
using static Mix.Cms.Lib.MixEnums;
using System.Linq.Expressions;
using System.Web;
using Mix.Cms.Lib.ViewModels.MixOrders;
using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Hub;
using Microsoft.Extensions.Caching.Memory;

namespace Mix.Cms.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/order")]
    public class ApiOrderController :
        BaseGenericApiControoler<MixCmsContext, MixOrder>
    {
        public ApiOrderController(IMemoryCache memoryCache, IHubContext<PortalHub> hubContext) : base(memoryCache, hubContext)
        {

        }
        #region Get

        // GET api/category/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixOrder>> DeleteAsync(int id)
        {
            var getOrder = await ReadListItemViewModel.Repository.GetSingleModelAsync(
                model => model.Id == id && model.Specificulture == _lang);
            if (getOrder.IsSucceed)
            {

                return await getOrder.Data.RemoveModelAsync(true);
            }
            else
            {
                return new RepositoryResponse<MixOrder>()
                {
                    IsSucceed = false
                };
            }
        }

        // GET api/orders/id
        [HttpGet, HttpOptions]
        [Route("details/{id}/{viewType}")]
        [Route("details/{viewType}")]
        public async Task<ActionResult<JObject>> Details(string viewType, int? id)
        {
            string msg = string.Empty;
            switch (viewType)
            {
               
                default:
                    if (id.HasValue)
                    {
                        var beResult = await ReadViewModel.Repository.GetSingleModelAsync(model => model.Id == id && model.Specificulture == _lang).ConfigureAwait(false);
                       
                        return JObject.FromObject(beResult);
                    }
                    else
                    {
                        var model = new MixOrder();
                        RepositoryResponse<ReadViewModel> result = new RepositoryResponse<ReadViewModel>()
                        {
                            IsSucceed = true,
                            Data = new ReadViewModel(model)
                            {
                                Specificulture = _lang,
                                Status = MixOrderStatus.Preview
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
                var result = await model.SaveModelAsync(true).ConfigureAwait(false);
                return result;
            }
            return new RepositoryResponse<UpdateViewModel>() { Status = 501 };
        }

        // POST api/category
        [HttpPost, HttpOptions]
        [Route("save/{id}")]
        public async Task<RepositoryResponse<MixOrder>> SaveFields(int id, [FromBody]List<EntityField> fields)
        {
            if (fields != null)
            {
                var result = new RepositoryResponse<MixOrder>() { IsSucceed = true };
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
            return new RepositoryResponse<MixOrder>();
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
            Expression<Func<MixOrder, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (!request.Status.HasValue || model.Status == request.Status.Value)
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || model.Customer.FullName.Contains(request.Keyword))
                        && (!request.FromDate.HasValue
                            || (model.CreatedDateTime >= request.FromDate.Value)
                        )
                        && (!request.ToDate.HasValue
                            || (model.CreatedDateTime <= request.ToDate.Value)
                        );

            switch (request.Key)
            {
                case "mvc":
                    var fedata = await ReadViewModel.Repository.GetModelListByAsync(predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);
                    return JObject.FromObject(fedata);
                case "portal":

                    var bedata = await UpdateViewModel.Repository.GetModelListByAsync(predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);
                    return JObject.FromObject(bedata);
                default:

                    var data = await ReadListItemViewModel.Repository.GetModelListByAsync(predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);
                    return JObject.FromObject(data);
            }
        }

        #endregion Post
    }
}