using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.Account
{
    public class MixAccountHelper
    {
        public static async Task<MixDatabaseDatas.AdditionalViewModel> LoadUserInfoAsync(string username,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var culture = MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var getInfo = await MixDatabaseDatas.Helper.LoadAdditionalDataAsync(MixDatabaseParentType.User, username, MixDatabaseNames.SYSTEM_USER_DATA
                    , culture, context, transaction);
                return getInfo.Data;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<MixDatabaseDatas.AdditionalViewModel>(ex, isRoot, transaction).Data;
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        public static List<NavUserRoleViewModel> GetRoleNavs(string userId)
        {
            UnitOfWorkHelper<MixCmsAccountContext>.InitTransaction(
                    null, null, out MixCmsAccountContext context,
                    out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var query = context.AspNetRoles
                  .Include(cp => cp.AspNetUserRoles)
                  .ToList()
                  .Select(p => new NavUserRoleViewModel()
                  {
                      UserId = userId,
                      RoleId = p.Id,
                      Description = p.Name,
                      IsActived = context.AspNetUserRoles.Any(m => m.UserId == userId && m.RoleId == p.Id)
                  })
                  .OrderBy(m => m.Priority)
                  .ToList();
                query.ForEach(m => m.ExpandView(context, transaction));
                return query;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsAccountContext>.HandleException<List<NavUserRoleViewModel>>(
                    ex, isRoot, transaction).Data;
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsAccountContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }
    }
}