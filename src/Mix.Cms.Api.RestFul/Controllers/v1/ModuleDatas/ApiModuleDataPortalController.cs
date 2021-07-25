// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixModuleDatas;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Identity.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/module-data/portal")]
    public class ApiModuleDataDataController :
        BaseAuthorizedRestApiController<MixCmsContext, MixModuleData, UpdateViewModel, UpdateViewModel, UpdateViewModel>
    {
        public ApiModuleDataDataController(
            DefaultRepository<MixCmsContext, MixModuleData, UpdateViewModel> repo,
            DefaultRepository<MixCmsContext, MixModuleData, UpdateViewModel> updRepo, 
            DefaultRepository<MixCmsContext, MixModuleData, UpdateViewModel> delRepo,
            MixIdentityHelper mixIdentityHelper,
            AuditLogRepository auditlogRepo) :
            base(repo, updRepo, delRepo, mixIdentityHelper, auditlogRepo)
        {
        }

        [HttpGet]
        public override async Task<ActionResult<PaginationModel<UpdateViewModel>>> Get()
        {
            bool isModuleId = int.TryParse(Request.Query["module_id"], out int moduleId);
            bool isPostId = int.TryParse(Request.Query["post_id"], out int postId);
            bool isPageId = int.TryParse(Request.Query[""], out int pageId);
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            string keyword = Request.Query[MixRequestQueryKeywords.Keyword];
            Expression<Func<MixModuleData, bool>> predicate = model =>
                model.Specificulture == _lang
                && (model.ModuleId == moduleId)
                && (!isPostId || model.PostId == postId)
                && (!isPageId || model.PageId == pageId)
                && (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(keyword)
                 || (EF.Functions.Like(model.Value, $"%{keyword}%"))
                 );
            var getData = await base.GetListAsync<UpdateViewModel>(predicate);
            if (getData.IsSucceed)
            {
                return getData.Data;
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        [HttpGet("export")]
        public async Task<ActionResult<FileViewModel>> Export()
        {
            bool isModuleId = int.TryParse(Request.Query["module_id"], out int moduleId);
            bool isPostId = int.TryParse(Request.Query["post_id"], out int postId);
            bool isPageId = int.TryParse(Request.Query[""], out int pageId);
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            string keyword = Request.Query[MixRequestQueryKeywords.Keyword];
            Expression<Func<MixModuleData, bool>> predicate = model =>
                model.Specificulture == _lang
                && (model.ModuleId == moduleId)
                && (!isPostId || model.PostId == postId)
                && (!isPageId || model.PageId == pageId)
                && (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(keyword)
                 || (EF.Functions.Like(model.Value, $"%{keyword}%"))
                 );
            var getData = await base.GetListAsync<UpdateViewModel>(predicate);
            var exportData = new List<JObject>();
            foreach (var item in getData.Data.Items)
            {
                item.JItem["created_date"] = new JObject()
                {
                    new JProperty("dataType", 1),
                    new JProperty("value", item.CreatedDateTime.ToLocalTime().ToString("dd-MM-yyyy hh:mm:ss"))
                };
                exportData.Add(item.JItem);
            }

            string exportPath = $"Exports/Module/{moduleId}";
            var result = MixCommonHelper.ExportJObjectToExcel(exportData, string.Empty, exportPath, Guid.NewGuid().ToString(), null);

            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        // GET api/module-data/create/id
        [HttpGet, HttpOptions]
        [Route("init-form/{moduleId}")]
        public async Task<ActionResult<UpdateViewModel>> InitByIdAsync(int moduleId)
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
                        Fields = getModule.Data.Fields,
                        Status = MixService.GetEnumConfig<MixContentStatus>(MixAppSettingKeywords.DefaultContentStatus),
                    });
                return Ok(ModuleData);
            }
            else
            {
                return BadRequest();
            }
        }

        // GET api/module-data/create/id
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
    }
}