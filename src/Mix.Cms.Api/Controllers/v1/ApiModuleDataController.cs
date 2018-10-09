// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mix.Domain.Core.ViewModels;
using Mix.Cms.Lib.Models.Cms;
using System.Linq.Expressions;
using Mix.Cms.Lib.ViewModels.MixModuleDatas;

namespace Mix.Cms.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/module-data")]
    public class ApiModuleDataController :
        BaseApiController
    {
        public ApiModuleDataController(Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(hubContext)
        {
        }

        // GET api/module-data/id
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
                        Fields = getModule.Data.Fields
                    };
                    return new RepositoryResponse<UpdateViewModel>() { IsSucceed = true, Data = await UpdateViewModel.InitViewAsync(model) };
                }
                else
                {
                    return new RepositoryResponse<UpdateViewModel>() { IsSucceed = false };
                }
            }
            else
            {
                var be = await UpdateViewModel.Repository.GetSingleModelAsync(model => model.Id == id && model.Specificulture == _lang);
                return be;
            }
        }

        // GET api/module-data/id
        [HttpGet, HttpOptions]
        [Route("edit/{id}")]
        public Task<RepositoryResponse<ReadViewModel>> Edit(string id)
        {
            return ReadViewModel.Repository.GetSingleModelAsync(model => model.Id == id && model.Specificulture == _lang);
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
        [HttpGet, HttpOptions]
        [Route("init-by-name/{moduleName}")]
        public async Task<RepositoryResponse<UpdateViewModel>> InitViewAsync(string moduleName)
        {
            var getModule = await Lib.ViewModels.MixModules.ReadListItemViewModel.Repository.GetSingleModelAsync(
                m => m.Name == moduleName && m.Specificulture == _lang).ConfigureAwait(false);
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

        // GET api/module-data/create/id
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
        public Task<RepositoryResponse<MixModuleData>> Delete(string id)
        {
            return ReadViewModel.Repository.RemoveModelAsync(model => model.Id == id && model.Specificulture == _lang);
        }

        #region Post

        // POST api/moduleData

        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Post([FromBody]UpdateViewModel data)
        {
            if (data != null)
            {
                var result = await data.SaveModelAsync(true).ConfigureAwait(false);
                return result;
            }
            return new RepositoryResponse<UpdateViewModel>();
        }

        // POST api/module
        [HttpPost, HttpOptions]
        [Route("save/{id}")]
        public async Task<RepositoryResponse<MixModuleData>> SaveFields(string id, [FromBody]List<EntityField> fields)
        {
            if (fields != null)
            {
                var result = new RepositoryResponse<MixModuleData>() { IsSucceed = true };
                foreach (var property in fields)
                {
                    if (result.IsSucceed)
                    {
                        result = await ReadViewModel.Repository.UpdateFieldsAsync(c => c.Id == id && c.Specificulture == _lang, fields).ConfigureAwait(false);
                    }
                    else
                    {
                        break;
                    }

                }
                return result;
            }
            return new RepositoryResponse<MixModuleData>();
        }



        // GET api/moduleData
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<RepositoryResponse<PaginationModel<ReadViewModel>>> GetList(
            [FromBody] RequestPaging request, int? level = 0)
        {
            int.TryParse(request.Key, out int moduleId);
            Expression<Func<MixModuleData, bool>> predicate = model =>
                model.Specificulture == _lang
                && model.ModuleId == moduleId
                //&& (string.IsNullOrWhiteSpace(request.Keyword)
                //    || (model.Title.Contains(request.Keyword)
                //    || model.Description.Contains(request.Keyword)))
                && (!request.FromDate.HasValue
                    || (model.CreatedDateTime >= request.FromDate.Value.ToUniversalTime())
                )
                && (!request.ToDate.HasValue
                    || (model.CreatedDateTime <= request.ToDate.Value.ToUniversalTime())
                )
                    ;

            var data = await ReadViewModel.Repository.GetModelListByAsync(predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);

            return data;
        }

        #endregion Post
    }
}