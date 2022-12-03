using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Base;
using System.Linq.Expressions;

namespace Mix.Portal.Domain.Services
{
    public sealed class CloneCultureService : TenantServiceBase
    {
        private MixCulture _destCulture;
        private MixCulture _srcCulture;
        private Dictionary<int, int> configurationIds = new();
        private Dictionary<int, int> languageIds = new();
        private Dictionary<int, int> pageIds = new();
        private Dictionary<int, int> postIds = new();
        private Dictionary<int, int> moduleIds = new();
        private Dictionary<Guid, Guid> dataIds = new();
        private Dictionary<Guid, Guid> valueIds = new();
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        public CloneCultureService(IHttpContextAccessor httpContextAccessor, UnitOfWorkInfo<MixCmsContext> cmsUOW)
            : base(httpContextAccessor)
        {
            _cmsUOW = cmsUOW;
        }

        #region Methods

        public async Task CloneDefaultCulture(string srcCulture, string destCulture)
        {
            try
            {
                _destCulture = _cmsUOW.DbContext.MixCulture.FirstOrDefault(m => m.Specificulture == destCulture && m.MixTenantId == CurrentTenant.Id);
                _srcCulture = _cmsUOW.DbContext.MixCulture.FirstOrDefault(m => m.Specificulture == srcCulture && m.MixTenantId == CurrentTenant.Id);

                if (_destCulture == null || srcCulture == null)
                {
                    throw new MixException(MixErrorStatus.Badrequest, $"{GetType().Name}:Invalid Culture");
                }

                await CloneIntegerData<MixConfigurationContent>(configurationIds);
                await CloneIntegerData<MixLanguageContent>(languageIds);

                await CloneIntegerData<MixPageContent>(pageIds);
                await CloneIntegerData<MixPostContent>(postIds);
                await CloneIntegerData<MixModuleContent>(moduleIds);
                await CloneModuleData();

                await CloneAssociations<MixPageModuleAssociation>(pageIds, moduleIds);
                await CloneAssociations<MixPagePostAssociation>(pageIds, postIds);
                await CloneAssociations<MixModulePostAssociation>(moduleIds, postIds);

                await CloneGuidData<MixDataContent>(dataIds);
                await CloneDataValues<MixDataContentValue>(valueIds);

                await CloneDataAssociations();
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        private async Task CloneIntegerData<T>(Dictionary<int, int> dictionaryIds)
            where T : MultilingualContentBase<int>
        {
            var contents = _cmsUOW.DbContext.Set<T>().Where(m => m.MixCultureId == _srcCulture.Id && m.MixTenantId == CurrentTenant.Id)
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

        private async Task CloneModuleData()
        {
            var contents = _cmsUOW.DbContext.MixModuleData.Where(m => m.MixCultureId == _srcCulture.Id && m.MixTenantId == CurrentTenant.Id)
                 .AsNoTracking()
                 .ToList();
            if (contents.Any())
            {
                var startId = GetStartIntegerId<MixModuleData>();
                foreach (var item in contents)
                {
                    startId++;
                    item.Id = startId;
                    item.ParentId = moduleIds[item.ParentId];
                    item.Specificulture = _destCulture.Specificulture;
                    item.MixCultureId = _destCulture.Id;
                    item.CreatedDateTime = DateTime.UtcNow;
                    await _cmsUOW.DbContext.MixModuleData.AddAsync(item);
                }
                await _cmsUOW.DbContext.SaveChangesAsync();
            }
        }

        private async Task CloneGuidData<T>(Dictionary<Guid, Guid> dictionaryIds)
            where T : MultilingualContentBase<Guid>
        {
            var contents = _cmsUOW.DbContext.Set<T>().Where(m => m.MixCultureId == _srcCulture.Id && m.MixTenantId == CurrentTenant.Id)
                 .AsNoTracking()
                 .ToList();
            if (contents.Any())
            {
                foreach (var item in contents)
                {
                    Guid newId = Guid.NewGuid();
                    dictionaryIds.Add(item.Id, newId);
                    item.Id = newId;
                    item.Specificulture = _destCulture.Specificulture;
                    item.MixCultureId = _destCulture.Id;
                    item.CreatedDateTime = DateTime.UtcNow;
                    await _cmsUOW.DbContext.Set<T>().AddAsync(item);
                }
                await _cmsUOW.DbContext.SaveChangesAsync();
            }
        }

        private async Task CloneDataValues<T>(Dictionary<Guid, Guid> dictionaryIds)
            where T : MultilingualContentBase<Guid>
        {
            var contents = _cmsUOW.DbContext.Set<T>().Where(m => m.MixCultureId == _srcCulture.Id && m.MixTenantId == CurrentTenant.Id)
                 .AsNoTracking()
                 .ToList();
            if (contents.Any())
            {
                foreach (var item in contents)
                {
                    if (dataIds.ContainsKey(item.ParentId))
                    {
                        Guid newId = Guid.NewGuid();
                        dictionaryIds.Add(item.Id, newId);
                        item.Id = newId;
                        item.ParentId = dataIds[item.ParentId];
                        item.Specificulture = _destCulture.Specificulture;
                        item.MixCultureId = _destCulture.Id;
                        item.CreatedDateTime = DateTime.UtcNow;
                        await _cmsUOW.DbContext.Set<T>().AddAsync(item);
                    }
                }
                await _cmsUOW.DbContext.SaveChangesAsync();
            }
        }

        private async Task CloneAssociations<T>(
                   Dictionary<int, int> leftDic,
                   Dictionary<int, int> rightDic)
                   where T : AssociationBase<int>
        {
            var data = _cmsUOW.DbContext.Set<T>().Where(m => m.MixTenantId == CurrentTenant.Id && leftDic.Keys.Contains(m.ParentId))
                .AsNoTracking()
                .ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    item.Id = 0;
                    item.ParentId = leftDic[item.ParentId];
                    item.ChildId = rightDic[item.ChildId];
                    await _cmsUOW.DbContext.Set<T>().AddAsync(item);
                }
                await _cmsUOW.DbContext.SaveChangesAsync();
            }
        }
        private async Task CloneDataAssociations()
        {
            Expression<Func<MixDataContentAssociation, bool>> predicate = m => false;
            predicate = predicate.OrIf(postIds.Count > 0, m => m.ParentType == MixDatabaseParentType.Post && m.IntParentId.HasValue && postIds.Keys.Contains(m.IntParentId.Value));
            predicate = predicate.OrIf(pageIds.Count > 0, m => m.ParentType == MixDatabaseParentType.Page && m.IntParentId.HasValue && pageIds.Keys.Contains(m.IntParentId.Value));
            predicate = predicate.OrIf(moduleIds.Count > 0, m => m.ParentType == MixDatabaseParentType.Module && m.IntParentId.HasValue && moduleIds.Keys.Contains(m.IntParentId.Value));
            predicate = predicate.OrIf(dataIds.Count > 0, m => m.ParentType == MixDatabaseParentType.MixDatabse && m.GuidParentId.HasValue && dataIds.Keys.Contains(m.GuidParentId.Value));
            predicate = predicate.AndAlso(m => m.MixTenantId == CurrentTenant.Id && m.Specificulture == _srcCulture.Specificulture);
            var data = _cmsUOW.DbContext.MixDataContentAssociation
                .Where(predicate)
                .AsNoTracking()
                .ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    item.Id = Guid.NewGuid();
                    item.DataContentId = dataIds[item.DataContentId];
                    item.IntParentId = GetIntegerParentId(item);
                    item.MixCultureId = _destCulture.Id;
                    item.Specificulture = _destCulture.Specificulture;
                    await _cmsUOW.DbContext.MixDataContentAssociation.AddAsync(item);
                }
                await _cmsUOW.DbContext.SaveChangesAsync();
            }
        }

        private int? GetIntegerParentId(MixDataContentAssociation item)
        {
            return item.ParentType switch
            {
                MixDatabaseParentType.Page => pageIds[item.IntParentId.Value],
                MixDatabaseParentType.Post => postIds[item.IntParentId.Value],
                MixDatabaseParentType.Module => moduleIds[item.IntParentId.Value],
                _ => null
            };
        }

        private int GetStartIntegerId<T>()
            where T : MultilingualContentBase<int>
        {
            return _cmsUOW.DbContext
                .Set<T>()
                .Max(m => m.Id);
        }

        #endregion
    }
}
