// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.SignalR.Hubs;
using Mix.Cms.Lib.ViewModels.MixModuleDatas;
using Mix.Heart.Models;
using Mix.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/module-data")]
    public class ApiModuleDataController :
        BaseGenericApiController<MixCmsContext, MixModuleData>
    {
        public ApiModuleDataController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        // GET api/module-data/id
        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("details/{viewType}/{moduleId}/{id}")]
        [Route("details/{viewType}/{moduleId}")]
        public async Task<RepositoryResponse<UpdateViewModel>> DetailsAsync(string viewType, int moduleId, string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                var getModule = await Lib.ViewModels.MixModules.ReadListItemViewModel.Repository.GetSingleModelAsync(
        m => m.Id == moduleId && m.Specificulture == _lang).ConfigureAwait(false);
                if (getModule.IsSucceed)
                {
                    var model = new MixModuleData(
                        )
                    {
                        ModuleId = moduleId,
                        Specificulture = _lang,
                        Fields = getModule.Data.Fields,
                        Status = MixService.GetEnumConfig<MixContentStatus>(MixAppSettingKeywords.DefaultContentStatus)
                    };
                    RepositoryResponse<UpdateViewModel> result = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_default", null, model);

                    return result;
                }
                else
                {
                    return new RepositoryResponse<UpdateViewModel>() { IsSucceed = false };
                }
            }
            else
            {
                Expression<Func<MixModuleData, bool>> predicate = model => model.Id == id && model.Specificulture == _lang;
                var portalResult = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_{id}", predicate);
                return portalResult;
            }
        }

        // GET api/module-data/id
        [HttpGet, HttpOptions]
        [Route("edit/{id}")]
        public Task<RepositoryResponse<ReadViewModel>> Edit(string id)
        {
            return base.GetSingleAsync<ReadViewModel>($"read_{id}", model => model.Id == id && model.Specificulture == _lang);
        }

        // GET api/module-data/create/id
        [HttpGet, HttpOptions]
        [Route("create/{moduleId}")]
        public async Task<RepositoryResponse<UpdateViewModel>> CreateAsync(int moduleId)
        {
            var getModule = await Lib.ViewModels.MixModules.ReadListItemViewModel.Repository.GetSingleModelAsync(
                m => m.Id == moduleId && m.Specificulture == _lang).ConfigureAwait(false);
            if (getModule.IsSucceed)
            {
                var ModuleData = new UpdateViewModel(
                    new MixModuleData()
                    {
                        ModuleId = moduleId,
                        Specificulture = _lang,
                        Fields = getModule.Data.Fields
                    });
                return new RepositoryResponse<UpdateViewModel>()
                {
                    IsSucceed = true,
                    Data = ModuleData
                };
            }
            else
            {
                return new RepositoryResponse<UpdateViewModel>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = getModule.Exception,
                    Errors = getModule.Errors
                };
            }
        }

        // GET api/module-data/create/id
        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("init-by-name/{moduleName}")]
        public async Task<RepositoryResponse<UpdateViewModel>> InitByName(string moduleName)
        {
            var getModule = await Lib.ViewModels.MixModules.ReadListItemViewModel.Repository.GetSingleModelAsync(
                m => m.Name == moduleName && m.Specificulture == _lang).ConfigureAwait(false);
            if (getModule.IsSucceed)
            {
                var moduleData = new UpdateViewModel(
                    new MixModuleData()
                    {
                        ModuleId = getModule.Data.Id,
                        Specificulture = _lang,
                        Fields = getModule.Data.Fields
                    });
                return new RepositoryResponse<UpdateViewModel>()
                {
                    IsSucceed = true,
                    Data = moduleData
                };
            }
            else
            {
                return new RepositoryResponse<UpdateViewModel>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = getModule.Exception,
                    Errors = getModule.Errors
                };
            }
        }

        // GET api/module-data/create/id
        [AllowAnonymous]
        [HttpPost, HttpOptions]
        [Route("save/{moduleName}")]
        public async Task<RepositoryResponse<UpdateViewModel>> SaveByName(string moduleName, [FromBody] JObject data)
        {
            var getModule = await Lib.ViewModels.MixModules.ReadListItemViewModel.Repository.GetSingleModelAsync(
                m => m.Name == moduleName && m.Specificulture == _lang).ConfigureAwait(false);
            if (getModule.IsSucceed)
            {
                var moduleData = new UpdateViewModel(
                    new MixModuleData()
                    {
                        ModuleId = getModule.Data.Id,
                        Specificulture = _lang,
                        Fields = getModule.Data.Fields
                    });
                foreach (var item in moduleData.DataProperties)
                {
                    moduleData.JItem[item.Name]["value"] = data[item.Name]?.Value<string>();
                }
                return await moduleData.SaveModelAsync();
            }
            else
            {
                return new RepositoryResponse<UpdateViewModel>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = getModule.Exception,
                    Errors = getModule.Errors
                };
            }
        }

        // GET api/module-data/create/id
        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("init/{moduleId}")]
        public async Task<RepositoryResponse<UpdateViewModel>> InitByIdAsync(int moduleId)
        {
            var getModule = await Lib.ViewModels.MixModules.ReadListItemViewModel.Repository.GetSingleModelAsync(
                m => m.Id == moduleId && m.Specificulture == _lang).ConfigureAwait(false);
            if (getModule.IsSucceed)
            {
                var ModuleData = new UpdateViewModel(
                    new MixModuleData()
                    {
                        ModuleId = getModule.Data.Id,
                        Specificulture = _lang,
                        Fields = getModule.Data.Fields
                    });
                return new RepositoryResponse<UpdateViewModel>()
                {
                    IsSucceed = true,
                    Data = ModuleData
                };
            }
            else
            {
                return new RepositoryResponse<UpdateViewModel>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = getModule.Exception,
                    Errors = getModule.Errors
                };
            }
        }

        // GET api/module-data/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixModuleData>> DeleteAsync(string id)
        {
            return await base.DeleteAsync<ReadViewModel>(model => model.Id == id && model.Specificulture == _lang);
        }

        #region Post

        // POST api/moduleData
        [AllowAnonymous]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Post([FromBody] UpdateViewModel data)
        {
            return await base.SaveAsync<UpdateViewModel>(data, true);
        }

        // GET api/moduleData
        [AllowAnonymous]
        [HttpPost, HttpOptions]
        [Route("export")]
        public async Task<ActionResult<RepositoryResponse<PaginationModel<ReadViewModel>>>> ExportData(
            [FromBody] RequestPaging request)
        {
            var query = HttpUtility.ParseQueryString(request.Query ?? "");
            int.TryParse(query.Get("module_id"), out int moduleId);
            int.TryParse(query.Get("post_id"), out int postId);
            int.TryParse(query.Get("category_id"), out int pageId);
            string key = $"{request.Key}_{request.PageSize}_{request.PageIndex}";

            Expression<Func<MixModuleData, bool>> predicate = model =>
                model.Specificulture == _lang
                && model.ModuleId == moduleId
                && (string.IsNullOrEmpty(request.Keyword) || model.Value.Contains(request.Keyword))
                && (postId == 0 || model.PostId == postId)
                && (pageId == 0 || model.PageId == pageId)
                && (!request.FromDate.HasValue
                    || (model.CreatedDateTime >= request.FromDate.Value.ToUniversalTime())
                )
                && (!request.ToDate.HasValue
                    || (model.CreatedDateTime <= request.ToDate.Value.ToUniversalTime())
                );

            var portalResult = await base.GetListAsync<ReadViewModel>(request, predicate);
            //foreach (var item in portalResult.Data.Items)
            //{
            //    item.JItem["created_date"] = new JObject()
            //    {
            //        new JProperty("dataType", 1),
            //        new JProperty("value", item.CreatedDateTime.ToLocalTime().ToString("dd-MM-yyyy hh:mm:ss"))
            //    };
            //    portalResult.Data.JsonItems.Add(item.JItem);
            //}

            //string exportPath = $"Exports/Module/{moduleId}";
            //var result = CommonHelper.ExportJObjectToExcel(portalResult.Data.JsonItems, string.Empty, exportPath, Guid.NewGuid().ToString(), null);
            return Ok(JObject.FromObject(portalResult));
        }

        // GET api/moduleData
        [AllowAnonymous]
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<RepositoryResponse<PaginationModel<ReadViewModel>>>> GetList(
            [FromBody] RequestPaging request, int? level = 0)
        {
            var query = HttpUtility.ParseQueryString(request.Query ?? "");
            int.TryParse(query.Get("module_id"), out int moduleId);
            int.TryParse(query.Get("post_id"), out int postId);
            int.TryParse(query.Get("product_id"), out int productId);
            int.TryParse(query.Get("category_id"), out int pageId);

            string key = $"{request.Key}_{request.PageSize}_{request.PageIndex}";
            Expression<Func<MixModuleData, bool>> predicate = model =>
                model.Specificulture == _lang
                && model.ModuleId == moduleId
                && (string.IsNullOrEmpty(request.Keyword) || model.Value.Contains(request.Keyword))
                && (postId == 0 || model.PostId == postId)
                && (pageId == 0 || model.PageId == pageId)
                && (!request.FromDate.HasValue
                    || (model.CreatedDateTime >= request.FromDate.Value.ToUniversalTime())
                )
                && (!request.ToDate.HasValue
                    || (model.CreatedDateTime <= request.ToDate.Value.ToUniversalTime())
                )
                    ;
            var portalResult = await base.GetListAsync<ReadViewModel>(request, predicate);

            return Ok(JObject.FromObject(portalResult));
        }

        // POST api/PortalPage
        [HttpPost, HttpOptions]
        [Route("update-infos")]
        public async Task<RepositoryResponse<List<ReadViewModel>>> UpdateInfos([FromBody] List<ReadViewModel> models)
        {
            if (models != null)
            {
                await MixCacheService.RemoveCacheAsync();
                return await ReadViewModel.UpdateInfosAsync(models);
            }
            else
            {
                return new RepositoryResponse<List<ReadViewModel>>();
            }
        }

        #endregion Post
    }
}