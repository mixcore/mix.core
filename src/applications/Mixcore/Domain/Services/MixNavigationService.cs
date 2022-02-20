using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Mixcore.Domain.Services
{
    public class MixNavigationService
    {
        private UnitOfWorkInfo uow;
        private MixCmsContext _dbContext;
        public MixNavigationService(MixCmsContext context)
        {
            _dbContext = context;
            uow = new(_dbContext);
        }

        public async Task<MixNavigation> GetNavigationAsync(string name, string culture, IUrlHelper Url)
        {
            var repo = NavigationViewModel.GetRepository(uow);
            string columnName = "name";
            Expression<Func<MixDataContentValue, bool>> keywordPredicate =
                           m => m.MixDatabaseColumnName == columnName && m.StringValue == name;
            var valDataIds = _dbContext.MixDataContentValue.Where(keywordPredicate).Select(m => m.ParentId).Distinct();
            var navs = await repo.GetListAsync(m => valDataIds.Contains(m.Id));
            var nav = navs?.FirstOrDefault()?.Nav;
            string activePath = Url.ActionContext.HttpContext.Request.Path;

            if (nav != null)
            {
                foreach (var cate in nav.MenuItems)
                {
                    cate.IsActive = cate.Uri == activePath;
                    if (cate.IsActive)
                    {
                        nav.ActivedMenuItem = cate;
                        nav.ActivedMenuItems.Add(cate);
                    }

                    foreach (var item in cate.MenuItems)
                    {
                        item.IsActive = item.Uri == activePath;
                        if (item.IsActive)
                        {
                            nav.ActivedMenuItem = item;
                            nav.ActivedMenuItems.Add(cate);
                            nav.ActivedMenuItems.Add(item);
                        }
                        cate.IsActive = cate.IsActive || item.IsActive;
                    }
                }
            }

            return nav;
        }
    }
}
