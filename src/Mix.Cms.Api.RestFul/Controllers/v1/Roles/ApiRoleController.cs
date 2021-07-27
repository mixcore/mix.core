// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.Account.MixRoles;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Identity.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/role")]
    [Route("api/v1/rest/{culture}/role")]
    public class ApiRoleController :
        BaseAuthorizedRestApiController<MixCmsAccountContext, AspNetRoles, UpdateViewModel, ReadViewModel, UpdateViewModel>
    {
        protected readonly RoleManager<IdentityRole> _roleManager;
        public ApiRoleController(
            DefaultRepository<MixCmsAccountContext, AspNetRoles, ReadViewModel> repo,
            DefaultRepository<MixCmsAccountContext, AspNetRoles, UpdateViewModel> updRepo,
            MixIdentityHelper mixIdentityHelper, RoleManager<IdentityRole> roleManager,
            AuditLogRepository auditlogRepo) 
            : base(repo, updRepo, updRepo, mixIdentityHelper, auditlogRepo)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            string keyword = Request.Query[MixRequestQueryKeywords.Keyword];
            Expression<Func<AspNetRoles, bool>> predicate = model =>
                (string.IsNullOrEmpty(keyword)
                 || model.Name.Contains(keyword)
                 || model.NormalizedName.Contains(keyword)
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = MixDefaultRoles.SuperAdmin)]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<IdentityRole>> Save([FromBody] string name)
        {
            var role = new IdentityRole()
            {
                Name = name,
                Id = Guid.NewGuid().ToString()
            };

            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded
                ? Ok(role)
                : BadRequest(result.Errors?.Select(e => $"{e.Code}: {e.Description}").ToList());
        }

        protected override async Task<RepositoryResponse<UpdateViewModel>> GetSingleAsync(string id)
        {
            var result = await base.GetSingleAsync(id);
            await result.Data?.LoadPermissions();
            return result;
        }

        public override async Task<ActionResult<UpdateViewModel>> Update(string id, [FromBody] UpdateViewModel data)
        {
            var result = await base.Update(id, data);
            await data.SavePermissionsAsync(data.Model);
            return result;
        }
    }
}