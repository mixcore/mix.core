using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Base;
using Mix.Portal.Domain.Interfaces;
using System.Linq.Expressions;

namespace Mix.Portal.Domain.Services
{
    public sealed class CloneCultureService : TenantServiceBase, ICloneCultureService
    {
        private MixCulture _destCulture;
        private MixCulture _srcCulture;
        private readonly Dictionary<int, int> _configurationIds = new();
        private readonly Dictionary<int, int> _languageIds = new();
        private readonly Dictionary<int, int> _pageIds = new();
        private readonly Dictionary<int, int> _postIds = new();
        private readonly Dictionary<int, int> _moduleIds = new();
        private readonly Dictionary<Guid, Guid> _dataIds = new();
        private readonly Dictionary<Guid, Guid> _valueIds = new();
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        public CloneCultureService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsContext> cmsUow) : base(httpContextAccessor)
        {
            _cmsUow = cmsUow;
        }

        #region Methods

        public async Task CloneDefaultCulture(string srcCulture, string destCulture, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _destCulture = _cmsUow.DbContext.MixCulture.FirstOrDefault(m => m.Specificulture == destCulture && m.MixTenantId == CurrentTenant.Id);
                _srcCulture = _cmsUow.DbContext.MixCulture.FirstOrDefault(m => m.Specificulture == srcCulture && m.MixTenantId == CurrentTenant.Id);

                if (_destCulture == null || srcCulture == null)
                {
                    throw new MixException(MixErrorStatus.Badrequest, $"{GetType().Name}:Invalid Culture");
                }

                await CloneIntegerData<MixConfigurationContent>(_configurationIds);
                await CloneIntegerData<MixLanguageContent>(_languageIds);

                await CloneIntegerData<MixPageContent>(_pageIds);
                await CloneIntegerData<MixPostContent>(_postIds);
                await CloneIntegerData<MixModuleContent>(_moduleIds);
                await CloneModuleData();

                await CloneAssociations<MixPageModuleAssociation>(_pageIds, _moduleIds);
                await CloneAssociations<MixPagePostAssociation>(_pageIds, _postIds);
                await CloneAssociations<MixModulePostAssociation>(_moduleIds, _postIds);

                await CloneGuidData<MixDataContent>(_dataIds);
                await CloneDataValues<MixDataContentValue>(_valueIds);

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
            var contents = _cmsUow.DbContext.Set<T>().Where(m => m.MixCultureId == _srcCulture.Id && m.MixTenantId == CurrentTenant.Id)
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
                    await _cmsUow.DbContext.Set<T>().AddAsync(item);
                }
                await _cmsUow.DbContext.SaveChangesAsync();
            }
        }

        private async Task CloneModuleData()
        {
            var contents = _cmsUow.DbContext.MixModuleData.Where(m => m.MixCultureId == _srcCulture.Id && m.MixTenantId == CurrentTenant.Id)
                 .AsNoTracking()
                 .ToList();
            if (contents.Any())
            {
                var startId = GetStartIntegerId<MixModuleData>();
                foreach (var item in contents)
                {
                    startId++;
                    item.Id = startId;
                    item.ParentId = _moduleIds[item.ParentId];
                    item.Specificulture = _destCulture.Specificulture;
                    item.MixCultureId = _destCulture.Id;
                    item.CreatedDateTime = DateTime.UtcNow;
                    await _cmsUow.DbContext.MixModuleData.AddAsync(item);
                }
                await _cmsUow.DbContext.SaveChangesAsync();
            }
        }

        private async Task CloneGuidData<T>(Dictionary<Guid, Guid> dictionaryIds)
            where T : MultilingualContentBase<Guid>
        {
            var contents = _cmsUow.DbContext.Set<T>().Where(m => m.MixCultureId == _srcCulture.Id && m.MixTenantId == CurrentTenant.Id)
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
                    await _cmsUow.DbContext.Set<T>().AddAsync(item);
                }
                await _cmsUow.DbContext.SaveChangesAsync();
            }
        }

        private async Task CloneDataValues<T>(Dictionary<Guid, Guid> dictionaryIds)
            where T : MultilingualContentBase<Guid>
        {
            var contents = _cmsUow.DbContext.Set<T>().Where(m => m.MixCultureId == _srcCulture.Id && m.MixTenantId == CurrentTenant.Id)
                 .AsNoTracking()
                 .ToList();
            if (contents.Any())
            {
                foreach (var item in contents)
                {
                    if (_dataIds.ContainsKey(item.ParentId))
                    {
                        Guid newId = Guid.NewGuid();
                        dictionaryIds.Add(item.Id, newId);
                        item.Id = newId;
                        item.ParentId = _dataIds[item.ParentId];
                        item.Specificulture = _destCulture.Specificulture;
                        item.MixCultureId = _destCulture.Id;
                        item.CreatedDateTime = DateTime.UtcNow;
                        await _cmsUow.DbContext.Set<T>().AddAsync(item);
                    }
                }
                await _cmsUow.DbContext.SaveChangesAsync();
            }
        }

        private async Task CloneAssociations<T>(
                   Dictionary<int, int> leftDic,
                   Dictionary<int, int> rightDic)
                   where T : AssociationBase<int>
        {
            var data = _cmsUow.DbContext.Set<T>().Where(m => m.MixTenantId == CurrentTenant.Id && leftDic.Keys.Contains(m.ParentId))
                .AsNoTracking()
                .ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    item.Id = 0;
                    item.ParentId = leftDic[item.ParentId];
                    item.ChildId = rightDic[item.ChildId];
                    await _cmsUow.DbContext.Set<T>().AddAsync(item);
                }
                await _cmsUow.DbContext.SaveChangesAsync();
            }
        }
        private async Task CloneDataAssociations()
        {
            Expression<Func<MixDataContentAssociation, bool>> predicate = m => false;
            predicate = predicate.OrIf(_postIds.Count > 0, m => m.ParentType == MixDatabaseParentType.Post && m.IntParentId.HasValue && _postIds.Keys.Contains(m.IntParentId.Value));
            predicate = predicate.OrIf(_pageIds.Count > 0, m => m.ParentType == MixDatabaseParentType.Page && m.IntParentId.HasValue && _pageIds.Keys.Contains(m.IntParentId.Value));
            predicate = predicate.OrIf(_moduleIds.Count > 0, m => m.ParentType == MixDatabaseParentType.Module && m.IntParentId.HasValue && _moduleIds.Keys.Contains(m.IntParentId.Value));
            predicate = predicate.OrIf(_dataIds.Count > 0, m => m.ParentType == MixDatabaseParentType.MixDatabse && m.GuidParentId.HasValue && _dataIds.Keys.Contains(m.GuidParentId.Value));
            predicate = predicate.AndAlso(m => m.MixTenantId == CurrentTenant.Id && m.Specificulture == _srcCulture.Specificulture);
            var data = _cmsUow.DbContext.MixDataContentAssociation
                .Where(predicate)
                .AsNoTracking()
                .ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    item.Id = Guid.NewGuid();
                    item.DataContentId = _dataIds[item.DataContentId];
                    item.IntParentId = GetIntegerParentId(item);
                    item.MixCultureId = _destCulture.Id;
                    item.Specificulture = _destCulture.Specificulture;
                    await _cmsUow.DbContext.MixDataContentAssociation.AddAsync(item);
                }
                await _cmsUow.DbContext.SaveChangesAsync();
            }
        }

        private int? GetIntegerParentId(MixDataContentAssociation item)
        {
            return item.ParentType switch
            {
                MixDatabaseParentType.Page => _pageIds[item.IntParentId.Value],
                MixDatabaseParentType.Post => _postIds[item.IntParentId.Value],
                MixDatabaseParentType.Module => _moduleIds[item.IntParentId.Value],
                _ => null
            };
        }

        private int GetStartIntegerId<T>()
            where T : MultilingualContentBase<int>
        {
            return _cmsUow.DbContext
                .Set<T>()
                .Max(m => m.Id);
        }

        #endregion
    }
}
