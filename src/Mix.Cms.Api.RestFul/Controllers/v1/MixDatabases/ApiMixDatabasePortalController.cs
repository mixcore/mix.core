// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.ViewModels.MixDatabases;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Identity.Helpers;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/v1/rest/mix-database/portal")]
    [Route("api/v1/rest/{culture}/mix-database/portal")]
    public class ApiMixDatabasePortalController :
        BaseAuthorizedRestApiController<MixCmsContext, MixDatabase, UpdateViewModel, ReadViewModel, UpdateViewModel>
    {
        public ApiMixDatabasePortalController(
            DefaultRepository<MixCmsContext, MixDatabase, ReadViewModel> repo, 
            DefaultRepository<MixCmsContext, MixDatabase, UpdateViewModel> updRepo, 
            DefaultRepository<MixCmsContext, MixDatabase, UpdateViewModel> delRepo,
            MixIdentityHelper mixIdentityHelper,
            AuditLogRepository auditlogRepo) :
            base(repo, updRepo, delRepo, mixIdentityHelper, auditlogRepo)
        {
        }

        // GET: api/v1/rest/en-us/mix-database/portal
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query["status"], out MixContentStatus status);
            bool isType = Enum.TryParse(Request.Query["type"], out MixDatabaseType type);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            string keyword = Request.Query["keyword"];
            Expression<Func<MixDatabase, bool>> predicate = model =>
                (!isStatus || model.Status == status)
                && (!isType || model.Type == type)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(keyword)
                 || model.Name.Contains(keyword)
                 || model.Title.Contains(keyword)
                 );
            var getData = await base.GetListAsync<ReadViewModel>(predicate);
            if (getData.IsSucceed)
            {
                return Ok(getData.Data);
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        [HttpGet("{id}")]
        public override async Task<ActionResult<UpdateViewModel>> Get(string id)
        {
            int.TryParse(id, out int dbId);
            var result = await _updRepo.GetSingleModelAsync(
                    m => (m.Id == dbId || m.Name == id));
            return Ok(result.Data);
        }
        
        [HttpPost("migrate/{id}")]
        public async Task<ActionResult> Migrate([FromBody] UpdateViewModel database)
        {
            var result = await Helper.MigrateDatabase(database, _lang);
            return result ? Ok() : BadRequest();
        }

        // DELETE: api/v1/rest/en-us/mix-database/portal/5
        [HttpDelete("{id}")]
        public override async Task<ActionResult<MixDatabase>> Delete(string id)
        {
            var getData = await DeleteViewModel.Repository.GetSingleModelAsync(m => m.Id == int.Parse(id));

            if (getData.IsSucceed)
            {
                var result = await getData.Data.RemoveModelAsync(true);
                if (result.IsSucceed)
                {
                    return Ok(result.Data);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}