using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixCultures;
using Mix.Common.Helper;
using Mix.Heart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixModules
{
    public class Helper
    {
        public static async Task<RepositoryResponse<bool>> Import(List<MixModule> arrModule, string destCulture,
           MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                int id = UpdateViewModel.ModelRepository.Max(m => m.Id, context, transaction).Data + 1;
                foreach (var item in arrModule)
                {
                    item.Id = id;
                    item.CreatedDateTime = DateTime.UtcNow;
                    item.Specificulture = destCulture;
                    context.MixModule.Add(item);
                    id++;
                }
                await context.SaveChangesAsync();
                result.Data = true;
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<ReadMvcViewModel>(ex, isRoot, transaction);
                result.IsSucceed = false;
                result.Errors = error.Errors;
                result.Exception = error.Exception;
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
            return result;
        }

        public static List<SupportedCulture> LoadCultures(int id, string initCulture = null, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getCultures = SystemCultureViewModel.Repository.GetModelList(_context, _transaction);
            var result = new List<SupportedCulture>();
            if (getCultures.IsSucceed)
            {
                foreach (var culture in getCultures.Data)
                {
                    result.Add(
                        new SupportedCulture()
                        {
                            Icon = culture.Icon,
                            Specificulture = culture.Specificulture,
                            Alias = culture.Alias,
                            FullName = culture.FullName,
                            Description = culture.FullName,
                            Id = culture.Id,
                            Lcid = culture.Lcid,
                            IsSupported = culture.Specificulture == initCulture || _context.MixModule.Any(p => p.Id == id && p.Specificulture == culture.Specificulture)
                        });
                }
            }
            return result;
        }

        public static RepositoryResponse<UpdateViewModel> GetBy(
            Expression<Func<MixModule, bool>> predicate, string postId = null, string productId = null, int pageId = 0
             , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = UpdateViewModel.Repository.GetSingleModel(predicate, _context, _transaction);
            if (result.IsSucceed)
            {
                result.Data.PostId = postId;
                result.Data.PageId = pageId;
            }
            return result;
        }
    }
}