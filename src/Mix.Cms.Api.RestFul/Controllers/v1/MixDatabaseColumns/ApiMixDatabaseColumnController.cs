// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.ViewModels.MixDatabaseColumns;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Identity.Helpers;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/mix-database-column/portal")]
    public class ApiMixDatabaseColumnController :
        BaseAuthorizedRestApiController<MixCmsContext, MixDatabaseColumn, UpdateViewModel, ReadViewModel, DeleteViewModel>
    {
        public ApiMixDatabaseColumnController(
            DefaultRepository<MixCmsContext, MixDatabaseColumn, ReadViewModel> repo, 
            DefaultRepository<MixCmsContext, MixDatabaseColumn, UpdateViewModel> updRepo, 
            DefaultRepository<MixCmsContext, MixDatabaseColumn, DeleteViewModel> delRepo, 
            MixIdentityHelper mixIdentityHelper,
            AuditLogRepository auditlogRepo) :
            base(repo, updRepo, delRepo, mixIdentityHelper, auditlogRepo)
        {
        }

        // GET: api/v1/rest/en-us/mix-database-column/client
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            string keyword = Request.Query[MixRequestQueryKeywords.Keyword];
            Expression<Func<MixDatabaseColumn, bool>> predicate = model =>
                (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(keyword)
                 || model.MixDatabaseName.Contains(keyword)
                 || model.Name.Contains(keyword)
                 || model.DefaultValue.Contains(keyword)
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

        // GET: api/v1/rest/en-us/mix-database-column/client
        [HttpGet("init/{mixDatabase}")]
        public async Task<ActionResult<PaginationModel<UpdateViewModel>>> Init(string mixDatabase)
        {
            int.TryParse(mixDatabase, out int mixDatabaseId);
            var getData = await UpdateViewModel.Repository.GetModelListByAsync(f => f.MixDatabaseName == mixDatabase || f.MixDatabaseId == mixDatabaseId
            , _context, _transaction);

            if (getData.IsSucceed)
            {
                return Ok(getData.Data);
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }
    }
}