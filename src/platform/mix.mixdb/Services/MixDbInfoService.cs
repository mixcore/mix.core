using Mix.Constant.Constants;
using Mix.Heart.Exceptions;
using Mix.Heart.Services;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.ViewModels;
using Mix.RepoDb.Models;
using Mix.Heart.UnitOfWork;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Service.Interfaces;
using Mix.Service.Services;
using Mix.Heart.Enums;

namespace Mix.Mixdb.Services
{
    /// <summary>
    /// Service for retrieving MixDb information
    /// </summary>
    public class MixDbInfoService : IMixDbInfoService
    {
        private readonly IMixMemoryCacheService _memoryCache;
        private readonly MixCacheService _cacheSrv;
        private readonly UnitOfWorkInfo<MixCmsContext> _uow;
        private readonly ILogger<MixDbInfoService> _logger;
        private MixDbTableViewModel? _mixDb;
        private FieldNameService? _fieldNameService;

        public MixDbInfoService(
            IMixMemoryCacheService memoryCache,
            MixCacheService cacheSrv,
            UnitOfWorkInfo<MixCmsContext> uow,
            ILogger<MixDbInfoService> logger)
        {
            _memoryCache = memoryCache;
            _cacheSrv = cacheSrv;
            _uow = uow;
            _logger = logger;
        }

        public async Task<MixDbTableViewModel?> GetMixDb(string tableName)
        {
            try
            {
                string name = $"{typeof(MixDbTableViewModel).FullName}_{tableName}";
                return await _memoryCache.TryGetValueAsync(
                    name,
                    cache =>
                    {
                        cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                        return MixDbTableViewModel.GetRepository(_uow, _cacheSrv).GetSingleAsync(m => m.SystemName == tableName);
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when getting MixDb configuration for table {tableName}");
                throw;
            }
        }

        public async Task LoadMixDb(string tableName)
        {
            try
            {
                if (_mixDb != null && _mixDb.SystemName == tableName)
                {
                    _fieldNameService = new FieldNameService(_mixDb.NamingConvention);
                    return;
                }

                _mixDb = await GetMixDb(tableName);
                if (_mixDb == null)
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Invalid table name");
                }
                _fieldNameService = new FieldNameService(_mixDb.NamingConvention);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when loading MixDb configuration for table {tableName}");
                throw;
            }
        }
    }
}