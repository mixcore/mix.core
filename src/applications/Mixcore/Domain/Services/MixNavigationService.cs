using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Mixcore.Domain.Services
{
    public class MixNavigationService
    {
        private readonly UnitOfWorkInfo _uow;
        private readonly MixCmsContext _dbContext;
        public MixNavigationService(MixCmsContext context)
        {
            _dbContext = context;
            _uow = new(_dbContext);
        }

        public Task<MixNavigation> GetNavigationAsync(string name, string culture, IUrlHelper Url)
        {
            // TODO: Update get nav
            return default;
        }
    }
}
