using Microsoft.Extensions.Caching.Memory;
using Mix.Service.Interfaces;

namespace Mix.Service.Services
{
    public class MixMemoryCacheService : IMixMemoryCacheService
    {
        public MemoryCache Cache { get; } = new MemoryCache(
            new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromSeconds(20)
            }
        );

        public ICacheEntry CreateEntry(object key)
        {
            return Cache.CreateEntry(key);
        }

        public void Dispose()
        {
            Cache.Dispose();
        }

        public void Remove(object key)
        {
            Cache.Remove(key);
        }

        public async Task<T?> TryGetValueAsync<T>(object key, Func<ICacheEntry, Task<T>> factory)
        {
            return await Cache.GetOrCreateAsync(key, factory);
        }
    }
}
