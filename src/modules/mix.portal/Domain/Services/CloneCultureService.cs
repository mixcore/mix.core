using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Base;
using Mix.Shared.Services;

namespace Mix.Portal.Domain.Services
{
    public class CloneCultureService
    {
        private UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        private MixService _mixService;
        private Dictionary<int, int> pageIds = new();
        private Dictionary<int, int> postIds = new();
        private Dictionary<int, int> moduleIds = new();
        private Dictionary<int, int> dataIds = new();
        public CloneCultureService(UnitOfWorkInfo<MixCmsContext> cmsUOW, MixService mixService)
        {
            _cmsUOW = cmsUOW;
            _mixService = mixService;
        }

        #region Methods

        public async Task CloneDefaultCulture(string specificulture)
        {
            string defaultCulture = GlobalConfigService.Instance.DefaultCulture;
            await CloneData<MixConfigurationContent, int>(defaultCulture, specificulture);
            await CloneData<MixLanguageContent, int>(defaultCulture, specificulture);

            await CloneData<MixPageContent, int>(defaultCulture, specificulture);
            await CloneData<MixPostContent, int>(defaultCulture, specificulture);
            await CloneData<MixModuleContent, int>(defaultCulture, specificulture);
            //await CloneData<MixDataContent, Guid>(defaultCulture, specificulture);
        }

        private async Task CloneData<T, TPrimaryKey>(string sourceCulture, string destCulture)
            where TPrimaryKey : IComparable
            where T : MultiLanguageContentBase<TPrimaryKey>
        {
            var contents = _cmsUOW.DbContext.Set<T>().Where(m => m.Specificulture == sourceCulture
                         && m.MixTenantId == _mixService.MixTenantId)
                 .AsNoTracking()
                 .ToList();
            foreach (var item in contents)
            {
                item.Id = default;
                item.Specificulture = destCulture;
                item.CreatedDateTime = DateTime.UtcNow;
                await _cmsUOW.DbContext.Set<T>().AddAsync(item);
            }
            await _cmsUOW.DbContext.SaveChangesAsync();
        }
        
        //private async Task CloneAssociations<T>(string sourceCulture, string destCulture)
        //    where T : AssociationBase<int>
        //{
        //    var contents = _cmsUOW.DbContext.Set<T>().Where(m => m.Specificulture == sourceCulture
        //                 && m.MixTenantId == _mixService.MixTenantId)
        //         .AsNoTracking()
        //         .ToList();
        //    foreach (var item in contents)
        //    {
        //        await _cmsUOW.DbContext.Set<T>().AddAsync(item);
        //    }
        //    await _cmsUOW.DbContext.SaveChangesAsync();
        //}

        #endregion
    }
}
