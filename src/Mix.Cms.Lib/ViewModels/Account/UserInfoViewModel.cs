using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Data.ViewModels;
using Mix.Identity.Models.AccountViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.ViewModels.Account
{
    public class UserInfoViewModel
        : ViewModelBase<MixCmsContext, MixCmsUser, UserInfoViewModel>
    {
        #region Properties

        //[JsonProperty("id")]

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

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        [JsonProperty("userRoles")]
        public List<UserRoleViewModel> UserRoles { get; set; } = new List<UserRoleViewModel>();

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain"); } }

        [JsonProperty("avatarUrl")]
        public string AvatarUrl {
            get {
                if (Avatar != null && (Avatar.IndexOf("http") == -1 && Avatar[0] != '/'))
                {
                    return CommonHelper.GetFullPath(new string[] {
                    Domain,  Avatar
                });
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
        }

        public UserInfoViewModel(MixCmsUser model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixCmsUser ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (MediaFile.FileStream != null)
            {
                MediaFile.FileFolder = CommonHelper.GetFullPath(new[] {
                    MixConstants.Folder.UploadFolder,
                    DateTime.UtcNow.ToString("MMM-yyyy")
                }); ;
                var isSaved = FileRepository.Instance.SaveWebFile(MediaFile);
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
            UserRoles = UserRoleViewModel.Repository.GetModelListBy(ur => ur.UserId == Id).Data;
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