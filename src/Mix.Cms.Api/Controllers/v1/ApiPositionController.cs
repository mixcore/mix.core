// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Lib.ViewModels.MixPositions;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/position")]
    public class ApiPositionController :
         BaseGenericApiController<MixCmsContext, MixPosition>
    {
        public ApiPositionController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/position/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixPosition>> DeleteAsync(int id)
        {
            return await base.DeleteAsync<UpdateViewModel>(
                model => model.Id == id, true);
        }

        // GET api/positions/id
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
                        Expression<Func<MixPosition, bool>> predicate = model => model.Id == id;
                        var portalResult = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_{id}", predicate);
                        if (portalResult.IsSucceed)
                        {
                            portalResult.Data.Pages = Lib.ViewModels.MixPagePositions.UpdateViewModel.Repository.GetModelListBy(p => p.PositionId == portalResult.Data.Id && p.Specificulture == _lang).Data;
                        }
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixPosition()
                        {
                            Status = MixService.GetConfig<int>("DefaultStatus"),
                            Priority = UpdateViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<UpdateViewModel> result = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
                default:
                    if (id.HasValue)
                    {
                        var beResult = await ReadViewModel.Repository.GetSingleModelAsync(model => model.Id == id).ConfigureAwait(false);
                        return Ok(JObject.FromObject(beResult));
                    }
                    else
                    {
                        var model = new MixPosition();
                        RepositoryResponse<ReadViewModel> result = new RepositoryResponse<ReadViewModel>()
                        {
                            IsSucceed = true,
                            Data = new ReadViewModel(model)
                            {
                                Specificulture = _lang,
                                Status = MixContentStatus.Preview
                            }
                        };

                        return Ok(JObject.FromObject(result));
                    }
            }
        }

        #endregion Get

        #region Post

        // POST api/position
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save([FromBody]UpdateViewModel model)
        {
            if (model != null)
            {
                var result = await base.SaveAsync<UpdateViewModel>(model, true);
                return result;
            }
            return new RepositoryResponse<UpdateViewModel>() { Status = 501 };
        }

        // POST api/position
        [HttpPost, HttpOptions]
        [Route("save/{id}")]
        public async Task<RepositoryResponse<MixPosition>> SaveFields(int id, [FromBody]List<EntityField> fields)
        {
            if (fields != null)
            {
                var result = new RepositoryResponse<MixPosition>() { IsSucceed = true };
                foreach (var property in fields)
                {
                    if (result.IsSucceed)
                    {
                        result = await ReadViewModel.Repository.UpdateFieldsAsync(c => c.Id == id, fields).ConfigureAwait(false);
                    }
                    else
                    {
                        break;
                    }
                }
                return result;
            }
            return new RepositoryResponse<MixPosition>();
        }

        // GET api/position
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            var query = HttpUtility.ParseQueryString(request.Query ?? "");
            bool isType = int.TryParse(query.Get("type"), out int positionType) && positionType >= 0;
            ParseRequestPagingDate(request);
            Expression<Func<MixPosition, bool>> predicate = model =>
                        (!request.Status.HasValue || model.Status == request.Status.Value)
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (model.Description.Contains(request.Keyword)
                            ));
            switch (request.Key)
            {
                case "mvc":
                    var mvcResult = await base.GetListAsync<ReadViewModel>(request, predicate);

                    return Ok(JObject.FromObject(mvcResult));

                case "portal":
                    var portalResult = await base.GetListAsync<UpdateViewModel>(request, predicate);

                    return Ok(JObject.FromObject(portalResult));

                default:

                    var listItemResult = await base.GetListAsync<ReadViewModel>(request, predicate);
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

        [HttpPost, HttpOptions]
        [Route("apply-list")]
        public async Task<ActionResult<JObject>> ListActionAsync([FromBody]ListAction<int> data)
        {
            Expression<Func<MixPosition, bool>> predicate = model =>
                       data.Data.Contains(model.Id);
            var result = new RepositoryResponse<bool>();
            switch (data.Action)
            {
                case "Delete":
                    return Ok(JObject.FromObject(await base.DeleteListAsync<UpdateViewModel>(predicate, true)));

                default:
                    return JObject.FromObject(new RepositoryResponse<bool>());
            }
        }

        #endregion Post
    }
}