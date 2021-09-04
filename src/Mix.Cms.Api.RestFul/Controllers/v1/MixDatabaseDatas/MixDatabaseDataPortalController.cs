// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.ViewModels.MixDatabaseDatas;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Identity.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Route("api/v1/rest/{culture}/mix-database-data/portal")]
    public class MixDatabaseDataPortalController :
        BaseAuthorizedRestApiController<MixCmsContext, MixDatabaseData, FormViewModel, FormViewModel, DeleteViewModel>
    {
        public MixDatabaseDataPortalController(
            DefaultRepository<MixCmsContext, MixDatabaseData, FormViewModel> repo, 
            DefaultRepository<MixCmsContext, MixDatabaseData, FormViewModel> updRepo, 
            DefaultRepository<MixCmsContext, MixDatabaseData, DeleteViewModel> delRepo, 
            MixIdentityHelper mixIdentityHelper,
            AuditLogRepository auditlogRepo) :
            base(repo, updRepo, delRepo, mixIdentityHelper, auditlogRepo)
        {
        }

        [HttpGet]
        public override async Task<ActionResult<PaginationModel<FormViewModel>>> Get()
        {
            var getData = await Helper.FilterByKeywordAsync<FormViewModel>(Request, _lang);
            if (getData.IsSucceed)
            {
                return Ok(getData.Data);
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        [HttpGet("additional-data")]
        public async Task<ActionResult<PaginationModel<AdditionalViewModel>>> GetAdditionalData()
        {
            if (Enum.TryParse(Request.Query["parentType"].ToString(), out MixDatabaseParentType parentType)
                && int.TryParse(Request.Query["parentId"].ToString(), out int parentId) && parentId > 0)
            {
                var getData = await Helper.GetAdditionalData(parentType, parentId.ToString(), Request, _lang);
                if (getData.IsSucceed)
                {
                    return Ok(getData.Data);
                }
                else
                {
                    return BadRequest(getData.Errors);
                }
            }
            else
            {
                var getAttrSet = await Lib.ViewModels.MixDatabases.UpdateViewModel.Repository.GetSingleModelAsync(
                    m => m.Name == Request.Query["databaseName"].ToString());
                if (getAttrSet.IsSucceed)
                {
                    AdditionalViewModel result = new AdditionalViewModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Specificulture = _lang,
                        MixDatabaseId = getAttrSet.Data.Id,
                        MixDatabaseName = getAttrSet.Data.Name,
                        Status = MixContentStatus.Published,
                        Columns = getAttrSet.Data.Columns,
                        ParentType = parentType,
                        CreatedDateTime = DateTime.UtcNow
                    };
                    result.ExpandView();
                    return Ok(result);
                }
                return BadRequest(getAttrSet.Errors);
            }
        }

        [HttpPost("save-additional-data")]
        public async Task<IActionResult> SaveAdditionalData([FromBody] AdditionalViewModel data)
        {
            if (string.IsNullOrEmpty(data.Id))
            {
                data.CreatedBy = User.Identity.Name;
            }
            else
            {
                data.ModifiedBy = User.Identity.Name;
                data.LastModified = DateTime.UtcNow;
            }

            var result = await base.SaveGenericAsync<AdditionalViewModel>(data, true);
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                var current = await GetSingleAsync(data.Id);
                if (!current.IsSucceed)
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
        }

        [HttpGet("init/{mixDatabase}")]
        public async Task<ActionResult<FormViewModel>> Init(string mixDatabase)
        {
            var formData = await Helper.GetFormDataAsync(mixDatabase, _lang);
            return formData != null
                ? Ok(formData)
                : BadRequest(mixDatabase);
        }

        [HttpPost("save-data/{mixDatabase}")]
        public async Task<ActionResult<FormViewModel>> SaveData([FromRoute]string mixDatabase, [FromBody] JObject data)
        {
            var formData = await Helper.GetFormDataAsync(mixDatabase, _lang);
            if (formData!=null)
            {
                formData.Obj = data;
                var result = await SaveAsync(formData, true);
                return GetResponse(result);
            }
            return BadRequest(mixDatabase);
        }

        [HttpGet("export")]
        public async Task<ActionResult> Export()
        {
            string mixDatabaseName = Request.Query["mixDatabaseName"].ToString();
            string exportPath = $"mix-content/exports/module/{mixDatabaseName}";
            var getData = await Helper.FilterByKeywordAsync<FormViewModel>(Request, _lang);

            var jData = new List<JObject>();
            if (getData.IsSucceed)
            {
                foreach (var item in getData.Data.Items)
                {
                    jData.Add(item.Obj);
                }
                var result = Lib.ViewModels.MixDatabaseDatas.Helper.ExportAttributeToExcel(jData, string.Empty, exportPath, $"{mixDatabaseName}", null);
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("import-data/{mixDatabaseName}")]
        public async Task<ActionResult<RepositoryResponse<ImportViewModel>>> ImportData(string mixDatabaseName, [FromForm] IFormFile file)
        {
            var getMixDatabase = await Lib.ViewModels.MixDatabases.ReadViewModel.Repository.GetSingleModelAsync(
                    m => m.Name == mixDatabaseName);
            if (getMixDatabase.IsSucceed)
            {
                if (file != null)
                {
                    var result = await Helper.ImportData(_lang, getMixDatabase.Data, file);
                    return Ok(result);
                }
            }
            return new RepositoryResponse<ImportViewModel>() { Status = 501 };
        }

        [HttpDelete("{id}")]
        public override async Task<ActionResult<MixDatabaseData>> Delete(string id)
        {
            var result = await DeleteAsync<DeleteViewModel>(id, true);
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }

        [HttpGet("migrate-data/{databaseId}")]
        public async Task<ActionResult> MigrateData(int databaseId)
        {
            var result = await Helper.MigrateData(databaseId, _lang);
            return result ? Ok() : BadRequest();
        }
    }
}