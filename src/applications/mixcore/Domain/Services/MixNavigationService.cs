using Microsoft.AspNetCore.Mvc;

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
            // TODO: PUT get nav
            return default;
        }
    }
}
