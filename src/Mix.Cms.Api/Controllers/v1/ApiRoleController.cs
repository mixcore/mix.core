using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Mix.Cms.Hub;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.ViewModels.Account;
using Mix.Cms.Lib.ViewModels.Account.MixRoles;
using Mix.Domain.Core.ViewModels;
using Mix.Identity.Models;
using Newtonsoft.Json.Linq;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/role")]
    public class ApiRoleController : BaseApiController
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly IEmailSender _emailSender;
        protected readonly ILogger _logger;

        public ApiRoleController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            ILogger<ApiRoleController> logger,
            IHubContext<PortalHub> hubContext) : base(hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("claims")]
        public object Claims()
        {

            return User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme
            , Roles = "SuperAdmin")]
        [HttpGet, HttpPost, HttpOptions]
        [Route("details/{id}/{viewType}")]
        [Route("details/{viewType}")]
        public async Task<JObject> GetDetails(string id, string viewType)
        {
            switch (viewType)
            {
                case "portal":
                    var beResult = await UpdateViewModel.Repository.GetSingleModelAsync(r => r.Id == id);
                    return JObject.FromObject(beResult);
                default:
                    var result = await RoleViewModel.Repository.GetSingleModelAsync(r => r.Id == id);
                    return JObject.FromObject(result);
            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet, HttpPost, HttpOptions]
        [Route("permissions")]
        public async Task<JObject> GetPermissions()
        {
            RepositoryResponse<List<ReadViewModel>> permissions = new RepositoryResponse<List<ReadViewModel>>()
            {
                IsSucceed = true,
                Data = new List<ReadViewModel>()
            };
            var roles = User.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").ToList();
            foreach (var item in roles)
            {
                var role = await _roleManager.FindByNameAsync(item.Value);
                var temp = await ReadViewModel.Repository.GetModelListByAsync(r => r.Id == role.Id);
                if (temp.IsSucceed)
                {
                    permissions.Data.AddRange(temp.Data);
                }
            }
            return JObject.FromObject(permissions);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
        [HttpGet, HttpPost, HttpOptions]
        [Route("list")]
        public async Task<RepositoryResponse<List<RoleViewModel>>> GetList()
        {
            return await RoleViewModel.Repository.GetModelListAsync();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
        [HttpPost, HttpOptions]
        [Route("create")]
        public async Task<RepositoryResponse<IdentityRole>> Save([FromBody]string name)
        {
            var role = new IdentityRole()
            {
                Name = name,
                Id = Guid.NewGuid().ToString()
            };

            var result = await _roleManager.CreateAsync(role);

            return new RepositoryResponse<IdentityRole>()
            {
                IsSucceed = result.Succeeded,
                Data = role,
                Errors = result.Errors?.Select(e => $"{e.Code}: {e.Description}").ToList()
            };
        }


        // POST api/role
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save(
            [FromBody] UpdateViewModel model)
        {
            if (model != null)
            {
                var result = await model.SaveModelAsync(true).ConfigureAwait(false);
                return result;
            }
            return new RepositoryResponse<UpdateViewModel>();
        }

        // POST api/role
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
        [HttpPost, HttpOptions]
        [Route("update-permission")]
        public async Task<RepositoryResponse<Lib.ViewModels.MixPortalPageRoles.ReadViewModel>> Update(
            [FromBody] Lib.ViewModels.MixPortalPageRoles.ReadViewModel model)
        {
            var result = new RepositoryResponse<Lib.ViewModels.MixPortalPageRoles.ReadViewModel>() { IsSucceed = true, Data = model };
            if (model != null)
            {
                if (model.IsActived)
                {
                    model.CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
                    var saveResult = await model.SaveModelAsync(false);
                    result.IsSucceed = saveResult.IsSucceed;

                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        result.Errors.AddRange(saveResult.Errors);

                    }
                }
                else
                {
                    var saveResult = await model.RemoveModelAsync(false);
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        result.Errors.AddRange(saveResult.Errors);
                    }
                }
                return result;
            }
            return new RepositoryResponse<Lib.ViewModels.MixPortalPageRoles.ReadViewModel>();
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
        [Route("delete/{name}")]
        public async Task<RepositoryResponse<AspNetRoles>> Delete(string name)
        {
            if (name != "SuperAdmin")
            {

                var result = await RoleViewModel.Repository.RemoveModelAsync(r => r.Name == name);
                return result;
            }
            else
            {
                return new RepositoryResponse<AspNetRoles>()
                {
                };
            }
        }


    }
}