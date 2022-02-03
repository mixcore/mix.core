using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Mix.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.Account
{
    public class MixRegisterViewModel : ViewModelBase<MixCmsContext, MixCmsUser, MixRegisterViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("createdby")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("status")]
        public MixUserStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("confirmPassword")]
        public string ConfirmPassword { get; set; }

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        [JsonProperty("userRoles")]
        public List<NavUserRoleViewModel> UserRoles { get; set; }

        [JsonProperty("domain")]
        public string Domain => MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain);

        [JsonProperty("avatarUrl")]
        public string AvatarUrl {
            get {
                if (Avatar != null && (Avatar.IndexOf("http") == -1 && Avatar[0] != '/'))
                {
                    return $"{Domain}/{Avatar}";
                }
                else
                {
                    return Avatar;
                }
            }
        }

        [JsonProperty("mediaFile")]
        public FileViewModel MediaFile { get; set; } = new FileViewModel();

        [JsonProperty("userData")]
        public JObject UserData { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public MixRegisterViewModel() : base()
        {
        }

        public MixRegisterViewModel(MixCmsUser model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixCmsUser ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (MediaFile.FileStream != null)
            {
                MediaFile.FileFolder = MixCmsHelper.GetUploadFolder();
                var isSaved = MixFileRepository.Instance.SaveWebFile(MediaFile);
                if (isSaved)
                {
                    Avatar = MediaFile.FullPath;
                }
                else
                {
                    IsValid = false;
                }
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UserRoles = GetRoleNavs();
        }

        #endregion Overrides

        #region Expands

        public List<NavUserRoleViewModel> GetRoleNavs()
        {
            using (MixCmsAccountContext context = new MixCmsAccountContext())
            {
                var query = context.AspNetRoles
                  .Include(cp => cp.AspNetUserRoles)
                  .ToList()
                  .Select(p => new NavUserRoleViewModel()
                  {
                      UserId = Id,
                      RoleId = p.Id,
                      Description = p.Name,
                      IsActived = context.AspNetUserRoles.Any(m => m.UserId == Id && m.RoleId == p.Id)
                  });

                return query.OrderBy(m => m.Priority).ToList();
            }
        }

        #endregion Expands
    }
}