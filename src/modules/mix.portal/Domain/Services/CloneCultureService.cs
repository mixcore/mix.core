using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Base;
using Mix.Shared.Services;

namespace Mix.Portal.Domain.Services
{
    public class CloneCultureService
    {
        private UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        private MixService _mixService;
        private MixCulture _destCulture;
        private MixCulture _srcCulture;
        private int _tenantId;
        private Dictionary<int, int> configurationIds = new();
        private Dictionary<int, int> languageIds = new();
        private Dictionary<int, int> pageIds = new();
        private Dictionary<int, int> postIds = new();
        private Dictionary<int, int> moduleIds = new();
        private Dictionary<Guid, Guid> dataIds = new();
        public CloneCultureService(UnitOfWorkInfo<MixCmsContext> cmsUOW, MixService mixService)
        {
            _cmsUOW = cmsUOW;
            _mixService = mixService;
            _tenantId = mixService.MixTenantId;
        }

        #region Methods

        public async Task CloneDefaultCulture(string srcCulture, string destCulture)
        {
            try
            {
                _destCulture = _cmsUOW.DbContext.MixCulture.FirstOrDefault(m => m.Specificulture == destCulture && m.MixTenantId == _tenantId);
                _srcCulture = _cmsUOW.DbContext.MixCulture.FirstOrDefault(m => m.Specificulture == srcCulture && m.MixTenantId == _tenantId);

                if (_destCulture == null || srcCulture == null)
                {
                    throw new MixException(MixErrorStatus.Badrequest, $"{GetType().Name}:Invalid Culture");
                }

                await CloneIntegerData<MixConfigurationContent>(configurationIds);
                await CloneIntegerData<MixLanguageContent>(languageIds);

                await CloneIntegerData<MixPageContent>(pageIds);
                await CloneIntegerData<MixPostContent>(postIds);
                await CloneIntegerData<MixModuleContent>(moduleIds);

                await CloneAssociations<MixPageModuleAssociation>(pageIds, moduleIds);
                await CloneAssociations<MixPagePostAssociation>(pageIds, postIds);
                await CloneAssociations<MixModulePostAssociation>(moduleIds, postIds);

                //await CloneData<MixDataContent, Guid>();

            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        private async Task CloneIntegerData<T>(Dictionary<int, int> dictionaryIds)
            where T : MultiLanguageContentBase<int>
        {
            var contents = _cmsUOW.DbContext.Set<T>().Where(m => m.MixCultureId == _srcCulture.Id && m.MixTenantId == _tenantId)
                 .AsNoTracking()
                 .ToList();
            if (contents.Any())
            {
                var startId = GetStartIntegerId<T>();
                foreach (var item in contents)
                {
                    startId++;
                    dictionaryIds.Add(item.Id, startId);
                    item.Id = startId;
                    item.Specificulture = _destCulture.Specificulture;
                    item.MixCultureId = _destCulture.Id;
                    item.CreatedDateTime = DateTime.UtcNow;
                    await _cmsUOW.DbContext.Set<T>().AddAsync(item);
                }
                await _cmsUOW.DbContext.SaveChangesAsync();
            }
        }
        private async Task CloneAssociations<T>(
                   Dictionary<int, int> leftDic,
                   Dictionary<int, int> rightDic)
                   where T : AssociationBase<int>
        {
            var data = _cmsUOW.DbContext.Set<T>().Where(m => m.MixTenantId == _tenantId && leftDic.Keys.Contains(m.LeftId))
                .AsNoTracking()
                .ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    item.Id = 0;
                    item.LeftId = leftDic[item.LeftId];
                    item.RightId = rightDic[item.RightId];
                    await _cmsUOW.DbContext.Set<T>().AddAsync(item);
                }
                await _cmsUOW.DbContext.SaveChangesAsync();
            }
        }

        private int GetStartIntegerId<T>()
            where T : MultiLanguageContentBase<int>
        {
            return _cmsUOW.DbContext
                .Set<T>()
                .Max(m => m.Id);
        }

        #endregion
    }
}
