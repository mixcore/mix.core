using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.Account.MixRoles
{
    public class ReadViewModel : ViewModelBase<MixCmsAccountContext, AspNetRoles, ReadViewModel>
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("concurrencyStamp")]
        public string ConcurrencyStamp { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("normalizedName")]
        public string NormalizedName { get; set; }

        #region Views

        [JsonProperty("permissions")]
        public List<MixPortalPages.UpdateRolePermissionViewModel> Permissions { get; set; }

        [JsonProperty("mixPermissions")]
        public List<MixDatabaseDatas.ReadMvcViewModel> MixPermissions { get; set; }
        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
            IsCache = false;
        }

        public ReadViewModel(AspNetRoles model, MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
            IsCache = false;
        }

        #endregion Contructors

        #region Overrides

        public override AspNetRoles ParseModel(MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
            }
            return base.ParseModel(_context, _transaction);
        }

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(ReadViewModel view, MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = await UserRoleViewModel.Repository.RemoveListModelAsync(false, ur => ur.RoleId == Id, _context, _transaction);
            return new RepositoryResponse<bool>()
            {
                IsSucceed = result.IsSucceed,
                Errors = result.Errors,
                Exception = result.Exception
            };
        }

        public override void ExpandView(MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
        {
           
        }

        #endregion Overrides

        #region Expands

        public async Task LoadPermissions(MixCmsContext _context = null
            , IDbContextTransaction _transaction = null)
        {
            var getPermissions = await MixPortalPages.UpdateRolePermissionViewModel.Repository.GetModelListByAsync(
                p => p.Level == 0
                && p.MixPortalPageRole.Any(r => r.RoleId == Id)
                , _context, _transaction);
            if (getPermissions.IsSucceed)
            {
                Permissions = getPermissions.Data;
                try
                {
                    foreach (var item in getPermissions.Data)
                    {
                        item.NavPermission = MixPortalPageRoles.ReadViewModel.Repository.GetSingleModel(
                            n => n.PageId == item.Id && n.RoleId == Id, _context, _transaction)
                            .Data;
                       
                        foreach (var child in item.ChildPages)
                        {
                            child.PortalPage.NavPermission = MixPortalPageRoles.ReadViewModel.Repository.GetSingleModel(
                                n => n.PageId == child.PortalPage.Id && n.RoleId == Id, _context, _transaction)
                                .Data;
                            if (child.PortalPage.NavPermission == null)
                            {
                                var nav = new MixPortalPageRole()
                                {
                                    PageId = child.PortalPage.Id,
                                    RoleId = Id,
                                    Status = MixContentStatus.Published
                                };
                                child.PortalPage.NavPermission = new MixPortalPageRoles.ReadViewModel(nav) { IsActived = false };
                            }
                            else
                            {
                                child.PortalPage.NavPermission.IsActived = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            await LoadMixPermissions(_context, _transaction);
        }

        public async Task LoadMixPermissions(MixCmsContext context = null, IDbContextTransaction transaction = null)
        {
            var getPermissions = await MixDatabaseDataAssociations.ReadMvcViewModel.Repository.GetModelListByAsync(
                   m => m.ParentType == MixDatabaseParentType.Role && m.ParentId == Id, context, transaction);
            MixPermissions = getPermissions.IsSucceed
                    ? getPermissions.Data.Select(n => n.Data).ToList()
                    : new List<MixDatabaseDatas.ReadMvcViewModel>();
        }

        #endregion Expands
    }
}