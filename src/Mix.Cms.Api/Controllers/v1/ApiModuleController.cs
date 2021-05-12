// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.SignalR.Hubs;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Lib.ViewModels.MixModules;
using Mix.Heart.Models;
using Mix.Identity.Constants;
using Mix.Identity.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/module")]
    public class ApiModuleController :
         BaseGenericApiController<MixCmsContext, MixModule>
    {
        private readonly MixIdentityHelper _idHelper;
        public ApiModuleController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<PortalHub> hubContext, MixIdentityHelper idHelper) : base(context, memoryCache, hubContext)
        {
            _idHelper = idHelper;
        }

        #region Get

        // GET api/module/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixModule>> DeleteAsync(int id)
        {
            return await base.DeleteAsync<UpdateViewModel>(
                model => model.Id == id && model.Specificulture == _lang, true);
        }

        // GET api/modules/id
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
                        Expression<Func<MixModule, bool>> predicate = model => model.Id == id && model.Specificulture == _lang;
                        var portalResult = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixModule()
                        {
                            Specificulture = _lang,
                            Status = MixService.GetEnumConfig<MixContentStatus>(MixAppSettingKeywords.DefaultContentStatus),
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
                            beResult.Data.LoadData();
                        }
                        return Ok(JObject.FromObject(beResult));
                    }
                    else
                    {
                        var model = new MixModule();
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

        // POST api/module
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save([FromBody] UpdateViewModel model)
        {
            if (model != null)
            {
                model.CreatedBy = _idHelper.GetClaim(User, MixClaims.Username);
                var result = await base.SaveAsync<UpdateViewModel>(model, true);
                return result;
            }
            return new RepositoryResponse<UpdateViewModel>() { Status = 501 };
        }

        // POST api/module
        [HttpPost, HttpOptions]
        [Route("save/{id}")]
        public async Task<RepositoryResponse<MixModule>> SaveFields(int id, [FromBody] List<EntityField> fields)
        {
            if (fields != null)
            {
                var result = new RepositoryResponse<MixModule>() { IsSucceed = true };
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
            return new RepositoryResponse<MixModule>();
        }

        // POST api/module
        [HttpPost, HttpOptions]
        [Route("data/save/{name}/{formName}")]
        public async Task<ActionResult<JObject>> SaveData(string name, string formName, [FromBody] JObject obj)
        {
            // Get module by name
            string _username = User?.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
            var result = await UpdateViewModel.SaveByModuleName(_lang, _username, name, formName, obj);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // GET api/module
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            var query = HttpUtility.ParseQueryString(request.Query ?? "");
            bool isType = int.TryParse(query.Get("type"), out int moduleType) && moduleType >= 0;
            ParseRequestPagingDate(request);
            Expression<Func<MixModule, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (string.IsNullOrEmpty(request.Status) || model.Status == Enum.Parse<MixContentStatus>(request.Status))
                        && (!isType || model.Type == moduleType)
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (EF.Functions.Like(model.Name, $"%{request.Keyword}%"))
                            || (EF.Functions.Like(model.Title, $"%{request.Keyword}%"))
                            || (EF.Functions.Like(model.Description, $"%{request.Keyword}%"))
                            )
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

                    return Ok(JObject.FromObject(mvcResult));

                case "portal":
                    var portalResult = await base.GetListAsync<UpdateViewModel>(request, predicate);

                    return Ok(JObject.FromObject(portalResult));

                default:

                    var listItemResult = await base.GetListAsync<ReadListItemViewModel>(request, predicate);
                    return JObject.FromObject(listItemResult);
            }
        }

        [HttpPost, HttpOptions]
        [Route("update-infos")]
        public async Task<RepositoryResponse<List<ReadListItemViewModel>>> UpdateInfos([FromBody] List<ReadListItemViewModel> models)
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
        public async Task<ActionResult<JObject>> ListActionAsync([FromBody] ListAction<int> data)
        {
            Expression<Func<MixModule, bool>> predicate = model =>
                       model.Specificulture == _lang
                       && data.Data.Contains(model.Id);
            var result = new RepositoryResponse<bool>();
            switch (data.Action)
            {
                case "Delete":
                    return Ok(JObject.FromObject(await base.DeleteListAsync<UpdateViewModel>(predicate, true)));

                case "Export":
                    return Ok(JObject.FromObject(await base.ExportListAsync(predicate, MixStructureType.Module)));

                default:
                    return JObject.FromObject(new RepositoryResponse<bool>());
            }
        }

        #endregion Post
    }
}