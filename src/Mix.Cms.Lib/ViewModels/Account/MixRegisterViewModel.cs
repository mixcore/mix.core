using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
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
        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        #endregion

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
        public string Domain => MixService.GetConfig<string>("Domain") ?? "/";

        [JsonProperty("avatarUrl")]
        public string AvatarUrl
        {
            get
            {
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
        #endregion

        #endregion

        #region Contructors

        public MixRegisterViewModel() : base()
        {
        }

        public MixRegisterViewModel(MixCmsUser model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion

        #region Overrides
        public override MixCmsUser ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (MediaFile.FileStream != null)
            {
                MediaFile.FileFolder = CommonHelper.GetFullPath(new[] {
                    MixService.GetConfig<string>("UploadFolder"),
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
            UserRoles = GetRoleNavs();
        }

        #endregion

        #region Expands

        public List<NavUserRoleViewModel> GetRoleNavs()
        {
            using (MixCmsAccountContext context = new MixCmsAccountContext())
            {
                var query = context.AspNetRoles
                  .Include(cp => cp.AspNetUserRoles)
                  .Select(p => new NavUserRoleViewModel()
                  {
                      UserId = Id,
                      RoleId = p.Id,
                      Specificulture = Specificulture,
                      Description = p.Name,
                      IsActived = context.AspNetUserRoles.Any(m => m.UserId == Id && m.RoleId == p.Id)
                  });

                return query.OrderBy(m => m.Priority).ToList();
            }

        }

        #endregion
    }
}
