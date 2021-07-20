using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Services;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Identity.Models.AccountViewModels;
using Mix.Infrastructure.Repositories;
using Mix.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.ViewModels.Account
{
    public class UserInfoViewModel
        : ViewModelBase<MixCmsAccountContext, AspNetUsers, UserInfoViewModel>
    {
        #region Properties

        #region Models

        public string Id { get; set; }
        public int AccessFailedCount { get; set; }
        public string Avatar { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int CountryId { get; set; }
        public string Culture { get; set; }
        public DateTime? Dob { get; set; }
        public string Email { get; set; }
        public ulong EmailConfirmed { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public ulong IsActived { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LastModified { get; set; }
        public string LastName { get; set; }
        public ulong LockoutEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public string ModifiedBy { get; set; }
        public string NickName { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public ulong PhoneNumberConfirmed { get; set; }
        public string RegisterType { get; set; }
        public string SecurityStamp { get; set; }
        public ulong TwoFactorEnabled { get; set; }
        public string UserName { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        [JsonProperty("userRoles")]
        public List<UserRoleViewModel> UserRoles { get; set; }

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain); } }

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

        [JsonProperty("resetPassword")]
        public ResetPasswordViewModel ResetPassword { get; set; }

        [JsonProperty("isChangePassword")]
        public bool IsChangePassword { get; set; }

        [JsonProperty("changePassword")]
        public ChangePasswordViewModel ChangePassword { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public UserInfoViewModel() : base()
        {
            IsCache = false;
        }

        public UserInfoViewModel(AspNetUsers model, MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
            IsCache = false;
        }

        #endregion Contructors

        #region Overrides

        public override AspNetUsers ParseModel(MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
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

        public override void ExpandView(MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
        {
            UserRoles ??= UserRoleViewModel.Repository.GetModelListBy(
                m => m.UserId == Id, _context, _transaction).Data;
            ResetPassword = new ResetPasswordViewModel();
        }

        #endregion Overrides
    }

    public class ChangePasswordViewModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}