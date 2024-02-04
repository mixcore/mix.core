using Microsoft.EntityFrameworkCore;
using Mix.Auth.Constants;
using Mix.Database.Entities.Account;
using Mix.Identity.Models.ManageViewModels;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.Services;
using RepoDb;
using System.Text.Json.Serialization;

namespace Mix.Lib.ViewModels
{
    public sealed class MixUserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool IsActived { get; set; }
        public System.DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }

        public string PhoneNumber { get; set; }

        public string Avatar { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }

        public JObject UserData { get; set; }

        public List<AspNetUserRoles> Roles { get; set; }
        public JArray PortalMenus { get; set; }

        public List<string> Endpoints { get; set; } = new();

        #region Change Password

        public ResetPasswordRequestModel ResetPassword { get; set; }

        public bool IsChangePassword { get; set; }

        public ChangePasswordViewModel ChangePassword { get; set; }
        [Newtonsoft.Json.JsonIgnore] private UnitOfWorkInfo<MixCmsContext> _cmsUow { get; }

        #endregion Change Password


        public MixUserViewModel(MixUser user, UnitOfWorkInfo<MixCmsContext> uow)
        {
            ReflectionHelper.Map(user, this);
            _cmsUow = uow;
        }

        public async Task LoadUserDataAsync(int tenantId, IMixDbDataService mixDbService,
            MixCmsAccountContext accContext, MixCacheService cacheService)
        {
            if (!GlobalConfigService.Instance.IsInit)
            {
                try
                {
                    UserData = await mixDbService.GetSingleByGuidParent(MixDatabaseNames.SYSTEM_USER_DATA, MixContentType.User, Id, true);
                    
                    var roles = from ur in accContext.AspNetUserRoles
                                join r in accContext.MixRoles
                                    on ur.RoleId equals r.Id
                                where ur.UserId == Id && ur.MixTenantId == tenantId
                                select ur;
                    Roles = await roles.ToListAsync();
                }
                catch (Exception ex)
                {
                    await MixLogService.LogExceptionAsync(ex);
                }
            }
        }

        public async Task LoadUserPortalMenus(string[] roles, int tenantId, MixRepoDbRepository repoDbRepository)
        {
            try
            {
                if (!roles.Contains(MixRoles.SuperAdmin))
                {
                    repoDbRepository.InitTableName(MixDatabaseNames.PORTAL_MENU);
                    var menus = await repoDbRepository.GetListByAsync(
                        new List<SearchQueryField>()
                        {
                            new SearchQueryField("Role", roles, MixCompareOperator.InRange)
                        }
                    );
                    PortalMenus = ReflectionHelper.ParseArray(menus);
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
                PortalMenus = new();
            }
        }

        //public async Task LoadUserEndpointsAsync(int tenantId, MixRepoDbRepository repoDbRepository)
        //{
        //    repoDbRepository.Init(MixDatabaseNames.SYSTEM_ENDPOINT);
        //    List<JObject> endpoints = new();
        //    foreach (var role in Roles)
        //    {
        //        var temp = await repoDbRepository.get

        //            GetByAllParent<MixDataContentViewModel>(
        //            new SearchDataContentModel(tenantId)
        //            {
        //                MixDatabaseName = MixDatabaseNames.SYSTEM_ENDPOINT,
        //                GuidParentId = role.RoleId
        //            });
        //        endpoints.AddRange(temp.Select(m => m.Data));
        //    }

        //    if (UserData != null && UserData.Data.ContainsKey("endpoints"))
        //    {
        //        var userEndpoints = UserData.Data.Value<JArray>("endpoints");
        //        endpoints.AddRange(userEndpoints.ToObject<List<JObject>>());
        //    }

        //    Endpoints = endpoints.Select(e =>
        //    {
        //        string method = e.Value<string>("method")?.ToLower();
        //        string path = e.Value<string>("path")?.ToLower();
        //        return $"{method} - {path}";
        //    }).Distinct().ToList();
        //}
    }
}