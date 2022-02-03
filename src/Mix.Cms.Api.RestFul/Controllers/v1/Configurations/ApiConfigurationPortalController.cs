// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.ViewModels.MixConfigurations;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Identity.Helpers;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/configuration")]
    public class ApiConfigurationPortalController :
        BaseAuthorizedRestApiController<MixCmsContext, MixConfiguration, UpdateViewModel, ReadMvcViewModel, UpdateViewModel>
    {
        public ApiConfigurationPortalController(
            DefaultRepository<MixCmsContext, MixConfiguration, ReadMvcViewModel> repo, 
            DefaultRepository<MixCmsContext, MixConfiguration, UpdateViewModel> updRepo, 
            DefaultRepository<MixCmsContext, MixConfiguration, UpdateViewModel> delRepo,
            MixIdentityHelper mixIdentityHelper,
            AuditLogRepository auditlogRepo) : 
            base(repo, updRepo, delRepo, mixIdentityHelper, auditlogRepo)
        {
        }
        
        [HttpGet("by-keyword/{keyword}")]
        public async Task<ActionResult<ReadMvcViewModel>> GetByKeyword(string keyword)
        {
            var config = await ReadMvcViewModel.Repository.GetSingleModelAsync(c => c.Keyword == keyword && c.Specificulture == _lang);
            return GetResponse(config);
        }


        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadMvcViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            string keyword = Request.Query[MixRequestQueryKeywords.Keyword];
            string category = Request.Query["category"];
            Expression<Func<MixConfiguration, bool>> predicate = model =>
                model.Specificulture == _lang
                && (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(keyword)
                 || model.Keyword.Contains(keyword)
                 || model.Value.Contains(keyword)
                 )
                 && (string.IsNullOrEmpty(category)
                 || model.Category == category
                 );
            var getData = await base.GetListAsync<ReadMvcViewModel>(predicate);
            return GetResponse(getData);
        }
    }
}