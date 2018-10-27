using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;


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

        #region Models

        #endregion

        #region Views

        [JsonProperty("permissions")]
        public List<MixPortalPages.ReadRolePermissionViewModel> Permissions { get; set; }

        #endregion

        #endregion

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(AspNetRoles model, MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion

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
            var result = await UserRoleViewModel.Repository.RemoveListModelAsync(ur => ur.RoleId == Id, _context, _transaction);
            return new RepositoryResponse<bool>()
            {
                IsSucceed = result.IsSucceed,
                Errors = result.Errors,
                Exception = result.Exception
            };
        }

        public override void ExpandView(MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
        {

            Permissions = MixPortalPages.ReadRolePermissionViewModel.Repository.GetModelListBy(p => p.Level == 0
            && (p.MixPortalPageRole.Any(r => r.RoleId == Id) || Name == "SuperAdmin")
            ).Data;
            foreach (var item in Permissions)
            {
                item.NavPermission = MixPortalPageRoles.ReadViewModel.Repository.GetSingleModel(n => n.PageId == item.Id && n.RoleId == Id).Data;

                foreach (var child in item.ChildPages)
                {
                    child.Page.NavPermission =  MixPortalPageRoles.ReadViewModel.Repository.GetSingleModel(n => n.PageId == child.Page.Id && n.RoleId == Id).Data;
                }
            }
        }

        #endregion

        #region Expands

        List<MixPortalPageRoles.ReadViewModel> GetPermission()
        {
            using (MixCmsContext context = new MixCmsContext())
            {
                var transaction = context.Database.BeginTransaction();
                var query = context.MixPortalPage
                .Include(cp => cp.MixPortalPageRole)
                .Select(Category =>
                new  MixPortalPageRoles.ReadViewModel(
                      new MixPortalPageRole()
                      {
                          RoleId = Id,
                          PageId = Category.Id,
                      }, context, transaction));

                var result = query.ToList();
                
                result.ForEach(nav =>
                {
                    nav.IsActived = context.MixPortalPageRole.Any(
                            m => m.PageId == nav.PageId && m.RoleId == Id);
                });
                transaction.Commit();
                return result.OrderBy(m => m.Priority).ToList();
            }
        }

        #endregion

    }
}
