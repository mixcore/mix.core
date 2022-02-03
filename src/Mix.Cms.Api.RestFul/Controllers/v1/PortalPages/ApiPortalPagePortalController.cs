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
using Mix.Cms.Lib.ViewModels.MixPortalPages;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Identity.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/permission")]
    public class ApiPortalPageController :
        BaseAuthorizedRestApiController<MixCmsContext, MixPortalPage, UpdateViewModel, ReadViewModel, UpdateViewModel>
    {
        public ApiPortalPageController(
            DefaultRepository<MixCmsContext, MixPortalPage, ReadViewModel> repo, 
            DefaultRepository<MixCmsContext, MixPortalPage, UpdateViewModel> updRepo, 
            DefaultRepository<MixCmsContext, MixPortalPage, UpdateViewModel> delRepo,
            MixIdentityHelper mixIdentityHelper,
            AuditLogRepository auditlogRepo)
            : base(repo, updRepo, delRepo, mixIdentityHelper, auditlogRepo)
        {
        }

        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            string keyword = Request.Query[MixRequestQueryKeywords.Keyword];
            var exceptIds = Request.Query["exceptIds"].ToString();
            var lstExceptIds = !string.IsNullOrEmpty(exceptIds) ? exceptIds.Split(',').Select(m => int.Parse(m)).ToList()
                : new List<int>();

            bool isLevel = int.TryParse(Request.Query["level"], out int level);
            Expression<Func<MixPortalPage, bool>> predicate = model =>
                (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (!isLevel || model.Level == level)
                && (string.IsNullOrEmpty(exceptIds) || !lstExceptIds.Any(m => m == model.Id))
                && (string.IsNullOrEmpty(keyword)
                 || (EF.Functions.Like(model.TextKeyword, $"%{keyword}%"))
                 || (EF.Functions.Like(model.TextDefault, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Url, $"%{keyword}%"))
                 );
            var getData = await base.GetListAsync<ReadViewModel>(predicate);
            if (getData.IsSucceed)
            {
                return getData.Data;
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }
    }
}