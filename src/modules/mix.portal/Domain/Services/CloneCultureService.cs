using Microsoft.EntityFrameworkCore;
using Mix.Shared.Services;

namespace Mix.Portal.Domain.Services
{
    public class CloneCultureService
    {
        private UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        private MixService _mixService;
        public CloneCultureService(UnitOfWorkInfo<MixCmsContext> cmsUOW, MixService mixService)
        {
            _cmsUOW = cmsUOW;
            _mixService = mixService;
        }

        #region Methods

        public async Task CloneDefaultCulture(string specificulture)
        {
            string defaultCulture = GlobalConfigService.Instance.DefaultCulture;
            await ClonePageContents(defaultCulture, specificulture);
        }

        private async Task ClonePageContents(string defaultCulture, string specificulture)
        {
            List<Task> tasks = new List<Task>();
            var pageContents = await MixPageContentViewModel.GetRepository(_cmsUOW)
                    .GetAllAsync(m => m.Specificulture == defaultCulture
                        && m.MixTenantId == _mixService.MixTenantId); ;
            foreach (var item in pageContents)
            {
                item.Id = 0;
                item.Specificulture = specificulture;
                tasks.Add(item.SaveAsync());
            }
            await Task.WhenAll(tasks);
        }

        #endregion
    }
}
