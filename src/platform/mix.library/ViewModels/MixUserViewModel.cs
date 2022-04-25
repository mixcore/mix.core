using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.Models.ManageViewModels;
using Mix.Lib.Dtos;
using Mix.Lib.Services;
using System.Text.Json.Serialization;

namespace Mix.Lib.ViewModels
{
    public class MixUserViewModel
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

        public FileModel MediaFile { get; set; } = new();

        public AdditionalDataContentViewModel UserData { get; set; }

        public List<AspNetUserRoles> Roles { get; set; }

        public List<string> Endpoints { get; set; } = new();

        #region Change Password

        public ResetPasswordViewModel ResetPassword { get; set; }

        public bool IsChangePassword { get; set; }

        public ChangePasswordViewModel ChangePassword { get; set; }
        [JsonIgnore]
        private UnitOfWorkInfo _cmsUow { get; }

        #endregion Change Password


        public MixUserViewModel(MixUser user, UnitOfWorkInfo uow)
        {
            ReflectionHelper.Mapping(user, this);
            _cmsUow = uow;
        }

        public async Task LoadUserDataAsync(int tenantId, MixDataService mixDataService)
        {
            UserData = await MixDataHelper.GetAdditionalDataAsync<AdditionalDataContentViewModel>(
                _cmsUow,
                MixDatabaseParentType.User,
                MixDatabaseNames.SYSTEM_USER_DATA,
                Id);
            UserData ??= await CreateDefaultUserData(tenantId, mixDataService);
            using var context = new MixCmsAccountContext();
            var roles = from ur in context.AspNetUserRoles
                        join r in context.MixRoles
                        on ur.RoleId equals r.Id
                        where ur.UserId == Id && ur.MixTenantId == tenantId
                        select ur;
            Roles = await roles.ToListAsync();

            await LoadUserEndpointsAsync(mixDataService);
        }

        private async Task<AdditionalDataContentViewModel> CreateDefaultUserData(int tenantId, MixDataService mixDataService)
        {
            var data = new AdditionalDataContentViewModel(_cmsUow)
            {
                MixTenantId = tenantId,
                Data = new JObject(),
                GuidParentId = Id,
                MixDatabaseName = MixDatabaseNames.SYSTEM_USER_DATA,
                ParentType = MixDatabaseParentType.User
            };
            await data.SaveAsync();
            return data;
        }


        public async Task LoadUserEndpointsAsync(MixDataService mixDataService)
        {
            List<JObject> endpoints = new();
            foreach (var role in Roles)
            {
                var temp = await mixDataService.GetByAllParent<MixDataContentViewModel>(new SearchMixDataDto()
                {
                    MixDatabaseName = MixDatabaseNames.SYSTEM_ENDPOINT,
                    GuidParentId = role.RoleId
                });
                endpoints.AddRange(temp.Select(m => m.Data));
            }

            if (UserData != null && UserData.Data.ContainsKey("endpoints"))
            {
                var userEndpoints = UserData.Data.Value<JArray>("endpoints");
                endpoints.AddRange(userEndpoints.ToObject<List<JObject>>());
            }

            Endpoints = endpoints.Select(e =>
            {
                string method = e.Value<string>("method")?.ToLower();
                string path = e.Value<string>("path")?.ToLower();
                return $"{method} - {path}";
            }).Distinct().ToList();
        }

    }
}
