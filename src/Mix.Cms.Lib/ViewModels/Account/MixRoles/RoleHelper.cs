using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.Account.MixRoles
{
    public class RoleHelper
    {
        public static async Task<List<MixPortalPages.UpdateRolePermissionViewModel>> LoadPermissions(string roleId, MixCmsContext _context = null
           , IDbContextTransaction _transaction = null)
        {
            var getPermissions = await MixPortalPages.UpdateRolePermissionViewModel.Repository.GetModelListByAsync(
                p => p.Level == 0
                && p.MixPortalPageRole.Any(r => r.RoleId == roleId)
                , _context, _transaction);
            if (getPermissions.IsSucceed)
            {
                try
                {
                    foreach (var item in getPermissions.Data)
                    {
                        item.NavPermission = MixPortalPageRoles.ReadViewModel.Repository.GetSingleModel(
                            n => n.PageId == item.Id && n.RoleId == roleId, _context, _transaction)
                            .Data;

                        foreach (var child in item.ChildPages)
                        {
                            child.PortalPage.NavPermission = MixPortalPageRoles.ReadViewModel.Repository.GetSingleModel(
                                n => n.PageId == child.PortalPage.Id && n.RoleId == roleId, _context, _transaction)
                                .Data;
                            if (child.PortalPage.NavPermission == null)
                            {
                                var nav = new MixPortalPageRole()
                                {
                                    PageId = child.PortalPage.Id,
                                    RoleId = roleId,
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
            return getPermissions.Data;
        }

        public static async Task<List<MixDatabaseDatas.ReadMvcViewModel>> LoadMixPermissions(
            string roleId, MixCmsContext context = null, IDbContextTransaction transaction = null)
        {
            var getPermissions = await MixDatabaseDataAssociations.ReadMvcViewModel.Repository.GetModelListByAsync(
                   m => m.ParentType == MixDatabaseParentType.Role && m.ParentId == roleId, context, transaction);
            return getPermissions.IsSucceed
                    ? getPermissions.Data.Select(n => n.Data).ToList()
                    : new List<MixDatabaseDatas.ReadMvcViewModel>();
        }
    }
}
